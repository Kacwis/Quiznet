using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class Player
    {
        public int Id { get; set; }
        
        public virtual User User { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public int Score { get; set; }

        public DateTime LastOnline { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
