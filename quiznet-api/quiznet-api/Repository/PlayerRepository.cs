using Microsoft.EntityFrameworkCore;
using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class PlayerRepository : Repository<Player>, IPlayerRepository
    {

        private readonly ApplicationDbContext _db;


        public PlayerRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        //public void ClearAllData()
        //{
        //    _db.Players.RemoveRange(_db.Players);
        //    _db.Users.RemoveRange(_db.Users);
        //    _db.SaveChanges();
        //}

        public async Task<Player> UpdateAsync(Player player)
        {
            var updatedPlayer = dbSet.Update(player);            
            await _db.SaveChangesAsync();
            return updatedPlayer.Entity;
        }


    }
}
