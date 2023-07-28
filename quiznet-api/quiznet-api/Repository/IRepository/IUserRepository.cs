using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
