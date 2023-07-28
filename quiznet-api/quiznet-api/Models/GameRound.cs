using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class GameRound
    {
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Game Game { get; set; }

        public virtual ICollection<PlayerAnswer> PlayerAnswers { get; set; }   

        [ForeignKey("Game")]
        public int GameId { get; set; }

        public int RoundNumber { get; set; }
        
    }
}
