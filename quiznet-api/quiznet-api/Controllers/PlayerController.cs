using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Handlers;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Numerics;
using System.Security.Claims;

namespace quiznet_api.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly IPlayerRepository _repository;

        protected APIResponse _response;

        private readonly IMapper _mapper;

        private readonly IUserService _userService;

        private readonly IJwtHandler _jwtHandler;

        private readonly IGameService _gameService;
        

        public PlayerController(
            IPlayerRepository repository,
            IMapper mapper,
            IJwtHandler jwtHandler,
            IUserService userService,
            IGameService gameService)
        {
            _repository = repository;
            _response = new APIResponse();
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _userService = userService;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllPlayers()
        {
            try
            {                
                _response.Result = await _repository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return _response;
            }
        }
        
        [HttpGet("startRandom/{playerId:int}")]
        public async Task<ActionResult<APIResponse>> FindRandomPlayerToPlay(int playerId)
        {
            try
            {
                var foundPlayer = await _repository.GetAsync(p => (p.Id != playerId) && (p.LastOnline > DateTime.Now.AddDays(-2)));
                if(foundPlayer == null)
                {
                    _response.Result = "Currently there are no players to play!";
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = foundPlayer;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpGet("menu/{id:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetMenuDataByPlayerId(int id)
        {
            try
            {                
                var player = await _repository.GetAsync(p => (p.Id == id));            
                if(player == null)
                {
                    _response.Result = "There are no players with that id";
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }                                
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                System.Diagnostics.Debug.WriteLine(userId + " " + player.User.Id + id);
                if (userId != player.User.Id)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }
                player.LastOnline = DateTime.Now;
                await _repository.UpdateAsync(player);
                await _repository.SaveAsync();
                var menuResponseDTO = await _userService.GetMenuResponseDTO(player);
                if (_jwtHandler.IsTokenCloseToExpire(HttpContext))
                {
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Role, "ROLE_USER"),
                        new Claim("user_id", player.User.Id.ToString())
                    };
                    var newToken = _jwtHandler.GenerateJwtToken(player.User.Username, claims, 20);
                    _response.Result = new { token = newToken, menuData = menuResponseDTO };
                }                
                
                _response.Result = new { menuData = menuResponseDTO};
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpGet("guest")]
        public async Task<ActionResult<APIResponse>> RegisterNewGuest()
        {
            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();                
                var guestRegistrationResponseDTO = await _userService.RegisterGuest(ipAddress);
                _response.Result = guestRegistrationResponseDTO;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        // Friendships endpoints
        
        [HttpPost("friend")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> SendFriendInvite([FromBody] CreateFriendshipDTO createFriendshipDTO)
        {
            try
            {
                var senderPlayer = await _repository.GetAsync(p => p.Id == createFriendshipDTO.SenderId);                     
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);                
                if (senderPlayer.User.Id != userId)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }
                var inviteMsg = senderPlayer.MessagesOutgoing.FirstOrDefault(m => m.ReceiverId == createFriendshipDTO.ReceiverId);
                if(inviteMsg != null && !inviteMsg.IsRead)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "You already send friend invite!" };
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                } 
                await _userService.SendFriendInvite(createFriendshipDTO);
                _response.Result = "Friend invite sent successfully!";
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpPost("friend/decision")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> MakeFriendshipDecision([FromBody] FriendshipDecisionDTO friendshipDecisionDTO)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var receiverPlayer = await _repository.GetAsync(p => p.Id == friendshipDecisionDTO.ReceiverId);
                
                if (receiverPlayer.User.Id != userId)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }
                if(friendshipDecisionDTO.Decision == "ACCEPT")
                {
                    await _userService.CreateFriendship(new CreateFriendshipDTO()
                    {
                        ReceiverId = friendshipDecisionDTO.ReceiverId,
                        SenderId = friendshipDecisionDTO.SenderId
                    });
                    _response.Result = "Friendship concluded successfully";
                    _response.StatusCode = System.Net.HttpStatusCode.Created;                    
                }
                else if(friendshipDecisionDTO.Decision == "REJECT")
                {
                    await _userService.RemoveMessage(new CreateFriendshipDTO()
                    {
                        ReceiverId = friendshipDecisionDTO.ReceiverId,
                        SenderId = friendshipDecisionDTO.SenderId
                    });
                    _response.Result = "Friendship rejected successfully";
                    _response.StatusCode = System.Net.HttpStatusCode.OK;
                }
                return Ok(_response);                
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpGet("findFriends/{username}")]
        public async Task<ActionResult<APIResponse>> FindPotentialFriends(string username)
        {
            try
            {                
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var potentialFriends = await _userService.FindPotentialFriends(userId, username);
                _response.Result = potentialFriends;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpPost("friend/block")]
        public async Task<ActionResult<APIResponse>> BlockFriend([FromBody] FriendshipActionDTO friendshipActionDto)
        {
            try
            {                
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var senderPlayer = await _repository.GetAsync(p => p.Id == friendshipActionDto.SenderId);
                if (senderPlayer == null || senderPlayer.UserId != userId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    return Unauthorized(_response);
                }
                await _userService.BlockFriend(senderPlayer, friendshipActionDto.FriendId);
                _response.Result = new { Message = "Friend blocked successfully" };
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }

        [HttpPost("unfriend")]
        public async Task<ActionResult<APIResponse>> RemoveFriendship([FromBody] FriendshipActionDTO friendshipActionDto)
        {
            try
            {                
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var senderPlayer = await _repository.GetAsync(p => p.Id == friendshipActionDto.SenderId);
                if (senderPlayer == null || senderPlayer.UserId != userId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    return Unauthorized(_response);
                }
                await _userService.RemoveFriendship(senderPlayer, friendshipActionDto.FriendId);
                _response.Result = new { Message = "Friend removed successfully" };
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }
        
        // Account options endpoints

        [HttpPut("{playerId:int}/avatar/{avatarId:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdateAvatarId(int playerId, int avatarId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _repository.GetAsync(p => p.UserId == userId);
                if (player == null || player.Id != playerId)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }

                player.AvatarId = avatarId;
                await _repository.UpdateAsync(player);
                _response.Result = new { Message = "Avatar id updated successfully" };
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }
        
        [HttpPut("{playerId:int}/username/{username}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdateUsername(int playerId, string username)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _repository.GetAsync(p => p.UserId == userId);
                if (player == null || player.Id != playerId)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }

                var foundPlayer = await _repository.GetAsync(p => p.User.Username == username);
                if (foundPlayer != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string>() { "This username is already in use" };
                    return Ok(_response);
                }
                var user = player.User;
                user.Username = username;
                await _userService.UpdateUserAsync(user);
                _response.Result = new { Message = "Username updated successfully" };
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }
        [HttpPut("{playerId:int}/password")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdatePassword([FromBody] string password, int playerId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _repository.GetAsync(p => p.UserId == userId);
                if (player == null || player.Id != playerId)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }

                await _userService.UpdatePassword(password, player.User);
                _response.Result = new { Message = "Username updated successfully" };
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { e.Message };
                return _response;
            }
        }
    }
}
