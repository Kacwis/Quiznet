namespace quiznet_api.Models.DTO
{
    public class StartGameDTO
    {
        public ICollection<Player> Players { get; set; }

        public CreateGameRoundDTO StartingRound { get; set; }


    }
}
