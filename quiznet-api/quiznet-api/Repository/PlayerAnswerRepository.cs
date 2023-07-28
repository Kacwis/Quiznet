using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class PlayerAnswerRepository : Repository<PlayerAnswer>, IPlayerAnswerRepository
    {
        private readonly ApplicationDbContext _db;

        public PlayerAnswerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Task<PlayerAnswer> UpdateAsync(PlayerAnswer playerAnswer)
        {
            throw new NotImplementedException();
        }
    }
}
