using quiznet_api.Models;

namespace quiznet_api.Repository.IRepository
{
    public interface IPlayerAnswerRepository : IRepository<PlayerAnswer>
    {

        Task<PlayerAnswer> UpdateAsync(PlayerAnswer playerAnswer);

    }
}
