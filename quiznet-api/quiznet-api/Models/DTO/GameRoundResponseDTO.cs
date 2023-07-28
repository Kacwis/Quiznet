using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models.DTO
{
    public class GameRoundResponseDTO
    {
        public int Id { get; set; }

        public Category Category { get; set; }
        
        public virtual ICollection<PlayerAnswerResponseDTO> PlayerAnswers { get; set; }
       
        public int RoundNumber { get; set; }
    }
}
