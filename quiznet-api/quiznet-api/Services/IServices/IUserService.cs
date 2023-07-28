using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Services.IServices
{
    public interface IUserService
    {
        Task<User> RegisterUser(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        bool IsUniqueUser(string username);
    }
}
