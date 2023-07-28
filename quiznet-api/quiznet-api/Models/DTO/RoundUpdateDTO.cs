namespace quiznet_api.Models.DTO
{
    public class RoundUpdateDTO
    {
        public ICollection<CreatePlayerAnswerDTO> PlayerAnswers { get; set; }
    }
}
