using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string TextPl { get; set; }
        
    }
}
