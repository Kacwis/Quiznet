namespace quiznet_api.Models.DTO
{
    public class GameResponseDTO
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public ICollection<GameRoundResponseDTO> Rounds { get; set; }

        public GameRoundResponseDTO ActiveRound { get; set; }

        public ICollection<GameResponsePlayerDTO> Players { get; set; }

        public DateTime CreationDate { get; set; }

        public int StartingPlayerId { get; set; }
    }
}
