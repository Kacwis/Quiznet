using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;
using System.Linq.Expressions;

namespace quiznet_api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _msgRepository;

        private readonly IPlayerRepository _playerRepository;

        public MessageService(IMessageRepository msgRepository, IPlayerRepository playerRepository)
        {
            _msgRepository = msgRepository;
            _playerRepository = playerRepository;
        }

        public async Task<Message> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var sender = await _playerRepository.GetAsync(p => p.Id == createMessageDTO.SenderId);
            var receiver = await _playerRepository.GetAsync(p => p.Id == createMessageDTO.ReceiverId);
            if(sender == null || receiver == null) 
            {
                throw new Exception("There are no players with that ids");
            }
            var newMessage = new Message()
            {
                Sender = sender,
                Receiver = receiver,
                SendAt = DateTime.UtcNow,
                IsRead = false,
                Text = createMessageDTO.Text
            };
            var createdMessage = await _msgRepository.CreateAsync(newMessage);
            return createdMessage;
        }
      
        public async Task UpdateIsMessageRead(int receiverId, Player senderPlayer)
        {
            foreach (var msg in senderPlayer.MessagesIncoming.Where(m => m.SenderId == receiverId))
            {
                msg.IsRead = true;
                await _msgRepository.UpdateAsync(msg);
            }
        }

        public List<ChatDTO> GetChatsForPlayer(Player player)
        {
            var chatReceivers = new HashSet<Player>();
            var chatsList = new List<ChatDTO>();
            foreach(var msg in player.MessagesOutgoing)
            {
                chatReceivers.Add(msg.Receiver);
            }
            foreach(var msg in player.MessagesIncoming)
            {
                chatReceivers.Add(msg.Sender);
            }
            foreach(var chatReceiver in chatReceivers)
            {
                var allMessages = new List<Message>();
                allMessages.AddRange(player.MessagesIncoming.ToList().FindAll(m => m.SenderId == chatReceiver.Id));
                allMessages.AddRange(player.MessagesOutgoing.ToList().FindAll(m => m.ReceiverId == chatReceiver.Id));
                var newChatDTO = new ChatDTO()
                {
                    ChatReceiver = new PlayerResponseDTO()
                    {
                        Id = chatReceiver.Id,
                        Score = chatReceiver.Score,
                        Username = chatReceiver.User.Username,
                        AvatarId = chatReceiver.AvatarId
                    },
                    LastMessage = GetMessageResponseDTO(allMessages.OrderByDescending(msg => msg.SendAt).FirstOrDefault())
                };
                chatsList.Add(newChatDTO);
            }
            return chatsList;
        }

        public async Task<WholeChatDTO> GetWholeChatByReceiver(int receiverId, Player player)
        {
            var receiver = await _playerRepository.GetAsync(p => p.Id == receiverId);
            if(receiver == null)
            {
                throw new Exception("There is no player with that id: " + receiverId);
            }
            var allMessages = new List<MessageResponseDTO>();
            player.MessagesIncoming.ToList()
                .FindAll(m => m.SenderId == receiverId)
                .ForEach(m => allMessages.Add(GetMessageResponseDTO(m)));
            player.MessagesOutgoing.ToList()
                .FindAll(m => m.ReceiverId == receiverId)
                .ForEach(m => allMessages.Add(GetMessageResponseDTO(m)));
            var wholeChatDTO = new WholeChatDTO()
            {
                ChatReceiver = new PlayerResponseDTO()
                {
                    Id = receiver.Id,
                    Score = receiver.Score,
                    Username = receiver.User.Username,
                },
                Messages = allMessages.OrderByDescending(m => m.SendAt).ToList()
            };
            return wholeChatDTO;
        }

        public async Task<Message> GetMessageAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _msgRepository.GetAsync(filter);
        }

        public async Task<List<Message>> GetAllMessagesAsync(Expression<Func<Message, bool>> filter = null)
        {
            return await _msgRepository.GetAllAsync(filter);
        }

        public async Task RemoveMessageAsync(Message message)
        {
            await _msgRepository.RemoveAsync(message);
        }

        
        private MessageResponseDTO GetMessageResponseDTO(Message message)
        {
            return new MessageResponseDTO()
            {
                Sender = new PlayerResponseDTO
                {
                    Id = message.SenderId,
                    Username = message.Sender.User.Username,
                    Score = message.Sender.Score
                },
                Receiver = new PlayerResponseDTO
                {
                    Id = message.ReceiverId,
                    Username = message.Receiver.User.Username,
                    Score = message.Receiver.Score
                },
                IsRead = message.IsRead,
                Text = message.Text,
                SendAt = message.SendAt
            };
        }

        
    }
}
