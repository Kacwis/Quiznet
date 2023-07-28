using quiznet_api.Data;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Repository
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
