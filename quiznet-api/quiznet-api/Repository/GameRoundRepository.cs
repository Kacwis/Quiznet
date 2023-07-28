using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class GameRoundRepository : Repository<GameRound>, IGameRoundRepository
    {
        private readonly ApplicationDbContext _db; 

        public GameRoundRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<GameRound> UpdateAsync(GameRound gameRound)
        {
            throw new NotImplementedException();
        }
    }
}
