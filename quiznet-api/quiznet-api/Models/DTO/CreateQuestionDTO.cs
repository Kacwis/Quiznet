namespace quiznet_api.Models.DTO
{
    public class CreateQuestionDTO
    {
        public string Text { get; set; }

        public string TextPl { get; set; }

        public ICollection<CreateAnswerDTO> Answers { get; set; }

        public int CategoryId { get; set; }

        public string CorrectAnswerText { get; set; }
    }
}
