using Microsoft.AspNetCore.Mvc;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace quiznet_api.Services
{
    public class UserService : IUserService
    {
        private static readonly string GUEST_PASSWORD = "guest1234";

        private static readonly int JWT_TOKEN_EXPIRE_TIME_MINUTES = 60 * 24;

        private readonly IUserRepository _userRepository;

        private readonly IPlayerRepository _playerRepository;

        private readonly IFriendshipRepository _friendshipRepository;

        private readonly IBlockedPlayerRepository _blockedPlayerRepository;

        private readonly IMessageService _messageService;

        private readonly IGameService _gameService;

        private readonly IJwtHandler _jwtHandler;

        public UserService(
            IUserRepository userRepository,
            IPlayerRepository playerRepository,
            IJwtHandler jwtHandler,
            IFriendshipRepository friendshipRepository,
            IMessageService messageService,
            IGameService gameService,
            IBlockedPlayerRepository blockedPlayerRepository)
        {
            _userRepository = userRepository;
            _playerRepository = playerRepository;
            _jwtHandler = jwtHandler;
            _friendshipRepository = friendshipRepository;   
            _messageService = messageService;
            _gameService = gameService;
            _blockedPlayerRepository = blockedPlayerRepository;
        }

        // Authentication logic
        
        public async Task<User> RegisterUser(RegistrationRequestDTO registrationRequestDTO) 
        {
            var registeredUser = await _userRepository.Register(registrationRequestDTO);
            var player = new Player()
            {                
                User = registeredUser,
                Score = 0,
                LastOnline = DateTime.Now,
                AvatarId = 1,
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
                loginResponseDTO.User = new PlayerResponseDTO()
                {
                    Username = player.User.Username,
                    Id = player.Id,
                    Score = player.Score,
                    AvatarId = player.AvatarId
                };
            }
            return loginResponseDTO;
        }

        public bool IsUniqueUser(string username)
        {
            return _userRepository.IsUniqueUser(username);
        }

        public async Task<LoginResponseDTO> RegisterGuest(string ipAddress)
        {
            var guestAccountMatchingIp = await _playerRepository.GetAsync(p => p.IpAddress == ipAddress);    
            
            var guestPlayer = guestAccountMatchingIp != null ? guestAccountMatchingIp : await RegisterNewGuest(ipAddress);

            var claims = new List<Claim>() { new Claim(ClaimTypes.Role, guestPlayer.User.Role), new Claim("user_id", guestPlayer.UserId.ToString()) };
            var jwtToken = _jwtHandler.GenerateJwtToken(guestPlayer.User.Username, claims, JWT_TOKEN_EXPIRE_TIME_MINUTES);            
            var userResponseDTO = new PlayerResponseDTO()
            {
                Id = guestPlayer.Id,
                Score = guestPlayer.Score,
                Username = guestPlayer.User.Username,
                AvatarId = guestPlayer.AvatarId
                
            };
            return new LoginResponseDTO()
            {
                User = userResponseDTO,
                Token = jwtToken,
            };
        }
        
        private async Task<Player> RegisterNewGuest(string ipAddress)
        {
            var allPlayers = await _playerRepository.GetAllAsync();
            var randomGuestName = "Guest" + GenerateRandomNumber(allPlayers.Count * 20);
            var email = "guest@quiznet.com";
            var registerDTO = new RegistrationRequestDTO()
            {
                Password = GUEST_PASSWORD,
                Email = email,
                Username = randomGuestName,
                Role = "ROLE_GUEST"
            };
            var registeredUser = await RegisterUser(registerDTO);
            var player = await _playerRepository.GetAsync(p => p.UserId == registeredUser.Id);
            if (ipAddress != null)
            {
                player.IpAddress = ipAddress;
            }
            await _playerRepository.UpdateAsync(player);
            await _playerRepository.SaveAsync();
            return player;            
        }

        // Friendships logic
        
        public async Task SendFriendInvite(CreateFriendshipDTO createFriendshipDTO)
        {
            var senderPlayer = await _playerRepository.GetAsync(p => p.Id == createFriendshipDTO.SenderId);
            var receiverPlayer = await _playerRepository.GetAsync(p => p.Id == createFriendshipDTO.ReceiverId);
            if(senderPlayer == null || receiverPlayer == null)
            {
                throw new Exception("There are no players with that ids");
            }
            var messageDTO = new CreateMessageDTO()
            {
                SenderId = senderPlayer.Id,
                ReceiverId = receiverPlayer.Id,
                Text = "PLAYER_INVITE"
            };
            await _messageService.CreateMessage(messageDTO);            
        }

        public async Task<List<FriendPlayerRequestDTO>> FindPotentialFriends(int userId, string username)
        {
            var player = await _playerRepository.GetAsync(p => p.UserId == userId);
            if(player == null)
            {
                throw new Exception("There is no player with that id");
            }
            var playerFriends = new List<Player>();
            player.FriendshipsOutgoing.ToList().ForEach(f => playerFriends.Add(f.Receiver));
            player.FriendshipsIncoming.ToList().ForEach(f => playerFriends.Add(f.Sender));
            var allPlayers = await _playerRepository
                .GetAllAsync(p => p.UserId != userId && p.User.Role != "ROLE_GUEST" && p.User.Username.Contains(username));
            var potentialFriends = new List<FriendPlayerRequestDTO>();
            foreach( var currPlayer in allPlayers.Except(playerFriends))
            {
                potentialFriends.Add(GetFriendPlayerDTO(currPlayer));
            }
            return potentialFriends;
        }

        public FriendPlayerRequestDTO GetFriendPlayerDTO(Player player)
        {
            var timeDifference = DateTime.Now - player.LastOnline;            
            return new FriendPlayerRequestDTO()
            {
                Id = player.Id,
                Username = player.User.Username,
                IsActive = timeDifference.TotalMinutes <= 2,
                Score = player.Score,
                AvatarId = player.AvatarId
            };
        }
        
        public async Task CreateFriendship(CreateFriendshipDTO createFriendshipDTO)
        {
            var sender = await _playerRepository.GetAsync(p => p.Id == createFriendshipDTO.SenderId);
            var receiver = await _playerRepository.GetAsync(p => p.Id == createFriendshipDTO.ReceiverId);  
            if(sender == null || receiver == null) 
            {
                throw new Exception("There are no players with that ids");
            }
            var newFriendship = new Friendship()
            {
                Sender = sender,
                Receiver = receiver,
                CreatedAt = DateTime.UtcNow,                
            };
            await _friendshipRepository.CreateAsync(newFriendship);
            await RemoveMessage(createFriendshipDTO);
        }

        public async Task BlockFriend(Player senderPlayer, int friendId)
        {
            await RemoveFriendship(senderPlayer, friendId);
            var friend = await _playerRepository.GetAsync(p => p.Id == friendId);
            if (friend == null)
            {
                throw new Exception("There are no players with that id");
            }
            var blockedPlayer = new BlockedPlayer()
            {
                BlockedAt = DateTime.Now,
                BlockingPlayerId = senderPlayer.Id,
                PlayerBlockedId = friend.Id,
            };

            await _blockedPlayerRepository.CreateAsync(blockedPlayer);
        }

        public async Task RemoveFriendship(Player senderPlayer, int friendId)
        {
            var friend = await _playerRepository.GetAsync(p => p.Id == friendId);
            if (friend == null)
            {
                throw new Exception("There is no player with that id");
            }

            var friendship = senderPlayer.FriendshipsIncoming.FirstOrDefault(f => f.SenderId == friend.Id);
            if (friendship == null)
            {
                friendship = senderPlayer.FriendshipsOutgoing.FirstOrDefault(f => f.ReceiverId == friend.Id);
            }
            await _friendshipRepository.RemoveAsync(friendship);
            var allMessagesToRemove = new List<Message>();
            allMessagesToRemove.AddRange(senderPlayer.MessagesIncoming.Where(m => m.SenderId == friend.Id));
            allMessagesToRemove.AddRange(senderPlayer.MessagesOutgoing.Where(m => m.ReceiverId == friend.Id));
            foreach (var msg in allMessagesToRemove)
            {
                await _messageService.RemoveMessageAsync(msg);
            }

            await _playerRepository.SaveAsync();
        }
        
        
        public async Task<MenuResponseDTO> GetMenuResponseDTO(Player player)
        {
            var activeGames = new List<GameResponseDTO>();
            var finishedGames = new List<GameResponseDTO>();
            var friends = new List<FriendPlayerRequestDTO>();
            foreach (var game in player.Games)
            {
                var currentGameResponse = await _gameService.GetGameResponseDTO(game);
                if (game.Status == "IN_PROGRESS")
                {
                    activeGames.Add(currentGameResponse);
                }
                else
                {
                    finishedGames.Add(currentGameResponse);
                }
            }
            foreach (var outgoingFriendship in player.FriendshipsOutgoing)
            {

                friends.Add(GetFriendPlayerDTO(outgoingFriendship.Receiver));

            }
            foreach (var incomingFriendship in player.FriendshipsIncoming)
            {
                friends.Add(GetFriendPlayerDTO(incomingFriendship.Sender));
            }
            var isNewMessages = false;
            foreach(var msg in player.MessagesIncoming)
            {
                if (!msg.IsRead)
                {
                    isNewMessages = true;
                }
            }
            return new MenuResponseDTO()
            {
                ActiveGames = activeGames,
                FinishedGames = finishedGames,
                Friends = friends,
                Player = new PlayerResponseDTO()
                {
                    Id = player.Id,
                    AvatarId = player.AvatarId,
                    Username = player.User.Username,
                    Score = player.Score
                },
                IsNewMessages = isNewMessages
            };
        }

        public async Task RemoveMessage(CreateFriendshipDTO createFriendshipDTO)
        {
            var inviteMessage = await _messageService.GetMessageAsync(
                m => m.SenderId == createFriendshipDTO.SenderId && m.ReceiverId == createFriendshipDTO.ReceiverId && m.Text == "PLAYER_INVITE");
            await _messageService.RemoveMessageAsync(inviteMessage);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdatePassword(string password, User user)
        {
            await _userRepository.UpdatePassword(password, user);
        }

        private static int GenerateRandomNumber(int length)
        {
            var random = new Random();
            return random.Next(1, length);
        }
    }
}
