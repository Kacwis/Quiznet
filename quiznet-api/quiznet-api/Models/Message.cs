namespace quiznet_api.Models
{
    public class Message
    {
        public int Id { get; set; } 

        public string Text { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public virtual Player Sender { get; set; }  

        public virtual Player Receiver { get; set; }

        public DateTime SendAt { get; set; }

        public bool IsRead { get; set; }


    }
}
