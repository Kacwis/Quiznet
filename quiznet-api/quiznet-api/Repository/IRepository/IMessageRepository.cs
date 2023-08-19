using quiznet_api.Models;

namespace quiznet_api.Repository.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task UpdateAsync(Message message);
    }
}
