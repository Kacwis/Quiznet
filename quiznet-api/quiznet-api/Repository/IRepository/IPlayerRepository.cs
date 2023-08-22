using quiznet_api.Models;

namespace quiznet_api.Repository.IRepository
{
    public interface IPlayerRepository : IRepository<Player>
    {        
        Task<Player> UpdateAsync(Player player);

        //void ClearAllData();
    }
}
