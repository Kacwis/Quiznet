namespace quiznet_api.Models.DTO
{
    public class MenuResponseDTO
    {
        public List<GameResponseDTO> ActiveGames { get; set; }

        public List<GameResponseDTO> FinishedGames { get; set; }
    }
}
