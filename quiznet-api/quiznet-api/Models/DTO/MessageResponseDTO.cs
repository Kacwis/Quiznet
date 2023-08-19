using Microsoft.Identity.Client;

namespace quiznet_api.Models.DTO
{
    public class MessageResponseDTO
    {
        public PlayerResponseDTO Sender { get; set; }

        public PlayerResponseDTO Receiver { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public DateTime SendAt { get; set; }
    }
}
