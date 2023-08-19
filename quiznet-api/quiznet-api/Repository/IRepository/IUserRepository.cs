using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task UpdateAsync(User user);

        Task UpdatePassword(string password, User user);
    }
}
