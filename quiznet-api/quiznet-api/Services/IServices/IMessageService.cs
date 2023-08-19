using quiznet_api.Models;
using quiznet_api.Models.DTO;
using System.Linq.Expressions;

namespace quiznet_api.Services.IServices
{
    
    public interface IMessageService
    {
        Task<Message> CreateMessage(CreateMessageDTO createMessageDTO);

        Task UpdateIsMessageRead(int receiverId, Player senderPlayer);

        List<ChatDTO> GetChatsForPlayer(Player player);

        Task<WholeChatDTO> GetWholeChatByReceiver(int receiverId, Player player);

        Task RemoveMessageAsync(Message message);

        Task<Message> GetMessageAsync(Expression<Func<Message, bool>> filter = null);

        Task<List<Message>> GetAllMessagesAsync(Expression<Func<Message, bool>> filter = null);




    }
}
