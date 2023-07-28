namespace quiznet_api.Models.DTO
{
    public class RegistrationRequestDTO
    {
        public string Username { get; set; }

        public string Email { get; set; }   

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
