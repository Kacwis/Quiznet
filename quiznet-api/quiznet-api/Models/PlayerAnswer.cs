using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class PlayerAnswer
    {
        public int Id { get; set; }
        
        public virtual Question Question { get; set; }

        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string SelectedAnswer { get; set; } 
            

        public int AnswerNumber { get; set; }
        

    }
}
