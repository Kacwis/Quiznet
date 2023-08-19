using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Services.IServices
{
    public interface IGameService
    {
        Task<ICollection<GameResponseDTO>> GetAllGameResponses();

        Task<Game> StartGameWithRandomPlayer(int playerId);
        
        Task<Game> GetGameById(int id);
        
        Task<GameResponseDTO> AddRoundToGame(CreateGameRoundDTO createGameRoundDTO, int gameId);

        Task DeleteGameById(int id);

        Task UpdateGameRound(int roundId, RoundUpdateDTO roundUpdateDTO);

        Task<GameResponseDTO> GetGameResponseDTO(Game game);

        Task<Game> StartGameWithFriend(CreateFriendGameDTO createFriendGameDto);
    }
}
