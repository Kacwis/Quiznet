namespace quiznet_api.Models.DTO
{
    public class CreatePlayerAnswerDTO
    {
        public int QuestionId { get; set; }

        public int PlayerId { get; set; }

        public string SelectedAnswer { get; set; }

        public int AnswerNumber { get; set; }
    }
}
