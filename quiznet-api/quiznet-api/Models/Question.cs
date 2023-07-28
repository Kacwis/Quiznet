using System.ComponentModel.DataAnnotations.Schema;

namespace quiznet_api.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string TextPl { get; set; }

        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int CorrectAnswerId { get; set; }

    }
}
