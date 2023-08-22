using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace quiznet_api.Services
{
    public class GameService : IGameService
    {
        private readonly int POINTS_FOR_THE_WIN = 10;

        private readonly int POINTS_FOR_DRAW = 3;
        
        private readonly IPlayerRepository _playerRepository;

        private readonly IGameRepository _gameRepository;

        private readonly IGameRoundRepository _gameRoundRepository;

        private readonly IPlayerAnswerRepository _playerAnswerRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IQuestionRepository _questionRepository;

        private readonly IMapper _mapper;

        public GameService(
            IPlayerRepository playerRepository, IGameRepository gameRepository,
            IGameRoundRepository gameRoundRepository, IMapper mapper,
            ICategoryRepository categoryRepository, IQuestionRepository questionRepository,
            IPlayerAnswerRepository playerAnswerRepository)
        {
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _gameRoundRepository = gameRoundRepository;
            _categoryRepository = categoryRepository;
            _questionRepository = questionRepository;
            _playerAnswerRepository = playerAnswerRepository;
            _mapper = mapper;
        }

        
        public async Task<ICollection<GameResponseDTO>> GetAllGameResponses()
        {
            var games = await _gameRepository.GetAllAsync();
            return _mapper.Map<ICollection<GameResponseDTO>>(games);
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var game = await _gameRepository.GetAsync(g => g.Id == gameId);
            if(game == null)
            {
                throw new Exception("Game not found!");
            }
            return game;
        }

        public async Task<Game> StartGameWithRandomPlayer(int playerId)
        {
            var player = await _playerRepository.GetAsync(p => p.Id == playerId);
            if(player == null)
            {
                throw new Exception("There are no players with that id");
            }
            var allPotentialPlayers =
                await _playerRepository.GetAllAsync(p => p.Id != player.Id && p.LastOnline > DateTime.Now.AddDays(-2));
            var potentialOpponent = allPotentialPlayers.FirstOrDefault<Player>(p => !ArePlayersHaveCurrentGame(player, p));
            if (potentialOpponent == null)
            {
                return null;    
            }
            var newGame = new Game()
            {
                Status = "IN_PROGRESS",
                Players = new List<Player>() { player, potentialOpponent },
                Rounds = new List<GameRound>(),
                StartingPlayerId = playerId
            };
            var createdGame = await _gameRepository.CreateAsync(newGame);
            await _gameRepository.SaveAsync();
            return createdGame;
        }
            

        public async Task<GameResponseDTO> AddRoundToGame(CreateGameRoundDTO gameRoundDTO, int gameId)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == gameRoundDTO.CategoryId);
            if(category == null)
            {
                throw new Exception("There is no category with that id: " + gameRoundDTO.CategoryId);
            }
            var game = await _gameRepository.GetAsync(g => g.Id == gameId);
            if (game == null)
            {
                throw new Exception("There is no game with that id: " + gameId);
            }
            var playerAnswers = new List<PlayerAnswer>();
            foreach(var playerAnswer in gameRoundDTO.PlayerAnswers)
            {
                playerAnswers.Add(await CreatePlayerAnswer(playerAnswer));
            }
            var newRound = new GameRound()
            {
                Category = category,
                PlayerAnswers = playerAnswers,
                RoundNumber = gameRoundDTO.RoundNumber,
                Game = game                
            };
            var createdGameRound = await _gameRoundRepository.CreateAsync(newRound);
            game.Rounds.Add(createdGameRound);
            game.CurrentRoundId = createdGameRound.Id;            
            await _gameRoundRepository.SaveAsync();
            var mappedGame = _mapper.Map<GameResponseDTO>(await _gameRepository.GetAsync(g => g.Id == gameId));
            return mappedGame;
        }

        private async Task<PlayerAnswer> CreatePlayerAnswer(CreatePlayerAnswerDTO createPlayerAnswerDTO)
        {
            var question = await _questionRepository.GetAsync(q => q.Id == createPlayerAnswerDTO.QuestionId);
            if(question == null)
            {
                throw new Exception("There is no question with that id: " + createPlayerAnswerDTO.QuestionId);
            }
            var player = await _playerRepository.GetAsync(p => p.Id == createPlayerAnswerDTO.PlayerId);
            if(player == null)
            {
                throw new Exception("There is no player with that id: " + createPlayerAnswerDTO.PlayerId);
            }
            var playerAnswer = new PlayerAnswer()
            {
                Player = player,
                Question = question,
                AnswerNumber = createPlayerAnswerDTO.AnswerNumber,
                SelectedAnswer = createPlayerAnswerDTO.SelectedAnswer
            };
            var createdPlayerAnswer = await _playerAnswerRepository.CreateAsync(playerAnswer);
            await _playerAnswerRepository.SaveAsync();
            return createdPlayerAnswer;
        }

        public async Task DeleteGameById(int gameId)
        {
            var game = await _gameRepository.GetAsync(g => g.Id == gameId);
            if(game == null)
            {
                throw new Exception("There is no game with that id: " + gameId);
            }
            var rounds = game.Rounds.ToList();
            foreach (var round in rounds)
            {
                foreach(var answer in round.PlayerAnswers)
                {
                    await _playerAnswerRepository.RemoveAsync(answer);
                }
                await _playerAnswerRepository.SaveAsync();
                await _gameRoundRepository.RemoveAsync(round);
            }
            await _gameRoundRepository.SaveAsync();
            await _gameRepository.RemoveAsync(game);
            await _gameRepository.SaveAsync();
        }

        public async Task UpdateGameRound(int roundId, RoundUpdateDTO roundUpdateDTO)
        {
            var round = await _gameRoundRepository.GetAsync(r => r.Id == roundId);
            if(round == null){
                throw new Exception("There is no Game round with that id: " + roundId);
            }
            foreach(var playerAnswer in roundUpdateDTO.PlayerAnswers)
            {
                var createdPlayerAnswer = await CreatePlayerAnswer(playerAnswer);
                round.PlayerAnswers.Add(createdPlayerAnswer);
            }
            if(round.Game.Rounds.Count == 5)
            {
                await FinishGame(round.Game);
            }
            await _gameRoundRepository.SaveAsync();            
        }

        private async Task FinishGame(Game game)
        {
            game.Status = "FINISHED";
            var player = game.Players.FirstOrDefault(p => p.Id == game.StartingPlayerId);
            var opponent = game.Players.FirstOrDefault(p => !p.Equals(player));
            var playerScore = GetScoreForPlayer(game, player);
            var opponentScore = GetScoreForPlayer(game, opponent);
            if (playerScore > opponentScore)
            {
                player.Score += 10;
            }
            else if (playerScore == opponentScore)
            {
                player.Score += 10;
                opponent.Score += 10;
            }
            else
            {
                opponent.Score += 10;
            }
            await _gameRoundRepository.SaveAsync();
        }

        private int GetScoreForPlayer(Game game, Player player)
        {
            var allPlayerAnswers = new List<PlayerAnswer>();
            var rounds = new List<GameRound>(game.Rounds);
            rounds.ForEach(r => allPlayerAnswers.AddRange(r.PlayerAnswers.Where(p => p.Player.Equals(player))));
            int score = 0;
            allPlayerAnswers.ForEach(a =>
            {
                var selectedAnswer = a.Question.Answers.FirstOrDefault(answer => answer.Text == a.SelectedAnswer);
                if (selectedAnswer.Id == a.Question.CorrectAnswerId) score++;
            } );
            return score;
        }
        

        public async Task<GameResponseDTO> GetGameResponseDTO(Game game)
        {
            var gameResponseDTO = _mapper.Map<GameResponseDTO>(game);
            gameResponseDTO.ActiveRound = _mapper.Map<GameRoundResponseDTO>(
                await _gameRoundRepository.GetAsync(r => r.Id == game.CurrentRoundId));
            return gameResponseDTO;
        }

        public async Task<Game> StartGameWithFriend(CreateFriendGameDTO createFriendGameDto)
        {
            var startingPlayer = await _playerRepository.GetAsync(p => p.Id == createFriendGameDto.StartingPlayerId);
            var friend = await _playerRepository.GetAsync(p => p.Id == createFriendGameDto.FriendId);
            if (startingPlayer == null || friend == null)
            {
                throw new Exception("There are no players with that ids");
            }

            var arePlayersInGame = ArePlayersHaveCurrentGame(startingPlayer, friend);
            if (arePlayersInGame)
            {
                var game = await _gameRepository.GetAsync(g =>
                    g.Players.Contains(startingPlayer) && g.Players.Contains(friend) && g.Status == "IN_PROGRESS");
                return game;
            }
            
            var newGame = new Game()
            {
                CreationDate = DateTime.Now,
                Players = new List<Player>() { startingPlayer, friend },
                Rounds = new List<GameRound>(),
                StartingPlayerId = startingPlayer.Id,
                Status = "IN_PROGRESS"
            };
            var createdGame = await _gameRepository.CreateAsync(newGame);
            return createdGame;
        }

        public bool ArePlayersHaveCurrentGame(Player player, Player opponent)
        {
            var gamesTogether = player.Games.FirstOrDefault(g => g.Players.Contains(opponent) && g.Status == "IN_PROGRESS");
            return gamesTogether != null;
        }
    }
}
