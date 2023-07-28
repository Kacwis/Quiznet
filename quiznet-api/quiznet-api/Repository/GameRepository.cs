using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly ApplicationDbContext _db;

        public GameRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }


        public Task<Game> UpdateAsync(Game game)
        {
            throw new NotImplementedException(); 
        }
    }
}
