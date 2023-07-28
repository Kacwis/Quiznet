using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;

namespace quiznet_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IPlayerRepository _playerRepository;

        public UserService(IUserRepository userRepository, IPlayerRepository playerRepository)
        {
            _userRepository = userRepository;
            _playerRepository = playerRepository;
        }

        public async Task<User> RegisterUser(RegistrationRequestDTO registrationRequestDTO) 
        {
            var registeredUser = await _userRepository.Register(registrationRequestDTO);
            var player = new Player()
            {                
                User = registeredUser,
                Score = 0,
                LastOnline = DateTime.Now,
            };
            await _playerRepository.CreateAsync(player);
            await _playerRepository.SaveAsync();
            return registeredUser;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {            
            var loginResponseDTO = await _userRepository.Login(loginRequestDTO);
            if(loginResponseDTO != null)
            {
                var player = await _playerRepository.GetAsync(p => p.User.Username == loginRequestDTO.Username);
                player.LastOnline = DateTime.Now;
                await _playerRepository.UpdateAsync(player);
                await _playerRepository.SaveAsync();
                loginResponseDTO.User = new LogInUserResponseDTO()
                {
                    Username = player.User.Username,
                    Id = player.Id,
                    Score = player.Score,
                };
            }
            return loginResponseDTO;
        }

        public bool IsUniqueUser(string username)
        {
            return _userRepository.IsUniqueUser(username);
        }
    }
}
