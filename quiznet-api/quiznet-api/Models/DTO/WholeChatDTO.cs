namespace quiznet_api.Models.DTO
{
    public class WholeChatDTO
    {
        public PlayerResponseDTO ChatReceiver { get; set; }

        public List<MessageResponseDTO> Messages { get; set; }
    }
}
