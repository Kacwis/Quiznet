using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Services.IServices
{
    public interface IQuestionService
    {
        Task<Question> CreateQuestion(CreateQuestionDTO createQuestionDTO);

        Task<ICollection<Question>> GetAllQuestions();

        Task DeleteQuestionById(int questionId);

        Task<ICollection<Question>> GetQuestionsByCategory(string categoryName);

        Task<ICollection<Question>> GetRandomQuestionsByCategory(int range, string categoryName);   
    }
}
