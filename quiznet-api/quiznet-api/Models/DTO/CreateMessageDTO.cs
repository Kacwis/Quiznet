namespace quiznet_api.Models.DTO
{
    public class CreateMessageDTO
    {
        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Text { get; set; } 
    }
}
