using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Services.IServices
{
    public interface IGameService
    {
        Task<ICollection<GameResponseDTO>> GetAllGames();

        Task<Game> StartGameWithRandomPlayer(int playerId);

        Task<GameResponseDTO> GetGameById(int id);

        Task<List<GameResponseDTO>> GetActiveGamesByPlayerId(int playerId);

        Task<GameResponseDTO> AddRoundToGame(CreateGameRoundDTO createGameRoundDTO, int gameId);

        Task DeleteGameById(int id);

        Task UpdateGameRound(int roundId, RoundUpdateDTO roundUpdateDTO);  
        
    }
}
