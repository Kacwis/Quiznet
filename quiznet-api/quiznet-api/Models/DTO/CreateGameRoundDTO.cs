namespace quiznet_api.Models.DTO
{
    public class CreateGameRoundDTO
    {
        public int RoundNumber { get; set; }    

        public int CategoryId { get; set; } 

        public ICollection<CreatePlayerAnswerDTO> PlayerAnswers { get; set; }
    }
}
