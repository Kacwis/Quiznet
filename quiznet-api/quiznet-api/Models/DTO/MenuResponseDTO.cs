namespace quiznet_api.Models.DTO
{
    public class MenuResponseDTO
    {
        public List<GameResponseDTO> ActiveGames { get; set; }

        public List<GameResponseDTO> FinishedGames { get; set; }

        public List<FriendPlayerRequestDTO> Friends { get; set; }   
        
        public PlayerResponseDTO Player { get; set; }

        public bool IsNewMessages { get; set; }
    }
}
