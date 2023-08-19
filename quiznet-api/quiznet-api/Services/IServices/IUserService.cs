using quiznet_api.Models;
using quiznet_api.Models.DTO;

namespace quiznet_api.Services.IServices
{
    public interface IUserService
    {
        Task<User> RegisterUser(RegistrationRequestDTO registrationRequestDTO);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<LoginResponseDTO> RegisterGuest(string ipAddress);

        bool IsUniqueUser(string username);

        Task SendFriendInvite(CreateFriendshipDTO createFriendshipDTO);

        Task<MenuResponseDTO> GetMenuResponseDTO(Player player);

        Task<List<FriendPlayerRequestDTO>> FindPotentialFriends(int userId, string username);

        FriendPlayerRequestDTO GetFriendPlayerDTO(Player player);

        Task CreateFriendship(CreateFriendshipDTO createFriendshipDTO);

        Task RemoveMessage(CreateFriendshipDTO createFriendshipDTO);

        Task UpdateUserAsync(User user);

        Task UpdatePassword(string password, User user);

        Task BlockFriend(Player senderPlayer, int friendId);

        Task RemoveFriendship(Player senderPlayer, int friendId);
        
        

    }
}
