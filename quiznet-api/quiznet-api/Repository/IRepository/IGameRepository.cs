using quiznet_api.Models;

namespace quiznet_api.Repository.IRepository
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<Game> UpdateAsync(Game game);
    }
}
