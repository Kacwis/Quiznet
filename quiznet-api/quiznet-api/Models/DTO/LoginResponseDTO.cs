namespace quiznet_api.Models.DTO
{
    public class LoginResponseDTO
    {
        public PlayerResponseDTO User { get; set; }

        public string Token { get; set; }   

    }
}
