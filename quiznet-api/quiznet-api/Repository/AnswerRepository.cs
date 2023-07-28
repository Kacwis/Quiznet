using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
