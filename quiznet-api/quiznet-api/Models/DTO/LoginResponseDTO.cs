namespace quiznet_api.Models.DTO
{
    public class LoginResponseDTO
    {

        public LogInUserResponseDTO User { get; set; }

        public string Token { get; set; }   

    }
}
