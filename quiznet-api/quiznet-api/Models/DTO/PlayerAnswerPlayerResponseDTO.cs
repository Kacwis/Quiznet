namespace quiznet_api.Models.DTO
{
    public class PlayerAnswerPlayerResponseDTO
    {
        public int Id { get; set; }

        public User User { get; set; }

        public int Score { get; set; }

        public DateTime LastOnline { get; set; }
    }
}
