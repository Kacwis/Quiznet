
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        
        public int ReceiverId { get; set; }
        
        public virtual Player Sender { get; set; }
      
        public virtual Player Receiver { get; set; }
    
        public DateTime CreatedAt { get; set; }
    }
}
