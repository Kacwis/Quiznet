using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public virtual ICollection<GameRound> Rounds { get; set; }

        public int CurrentRoundId { get; set; }        

        public virtual ICollection<Player> Players { get; set; } 

        public DateTime CreationDate { get; set; }

        public int StartingPlayerId { get; set; }

    }
}
