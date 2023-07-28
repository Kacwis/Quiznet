using quiznet_api.Models;

namespace quiznet_api.Repository.IRepository
{
    public interface IGameRoundRepository : IRepository<GameRound>
    {

        Task<GameRound> UpdateAsync(GameRound gameRound);   
    }
}
