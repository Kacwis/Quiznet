using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models.DTO
{
    public class PlayerAnswerResponseDTO
    {       
        public Question Question { get; set; }

        public PlayerAnswerPlayerResponseDTO Player { get; set; }

        public string SelectedAnswer { get; set; }

        public int AnswerNumber { get; set; }

        public int IsCorrect { get; set; }

    }
}
