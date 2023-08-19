using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Handlers.IHandlers;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;

namespace quiznet_api.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        private readonly IPlayerRepository _playerRepository;

        private readonly IJwtHandler _jwtHandler;

        protected APIResponse _response;

        public MessageController(IMessageService service, IJwtHandler jwtHandler, IPlayerRepository playerRepository)
        {
            _service = service;
            _response = new APIResponse();
            _jwtHandler = jwtHandler;
            _playerRepository = playerRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<APIResponse>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _playerRepository.GetAsync(p => p.UserId == userId);
                if(player != null && player.Id != createMessageDTO.SenderId)
                {
                    _response.Result = new { Message = "You don't have access to this content" };
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                await _service.CreateMessage(createMessageDTO);
                _response.Result = new { Message = "Message sent successfully!" };
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("{receiverId:int}")]
        public async Task<ActionResult<APIResponse>> UpdateIsMessageRead(int receiverId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var senderPlayer = await _playerRepository.GetAsync(p => p.UserId == userId);
                if (senderPlayer == null)
                {
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }
                await _service.UpdateIsMessageRead(receiverId, senderPlayer);
                _response.Result = new { Message = "Message updated successfully!" };
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet("chats/{playerId:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetChatsByPlayer(int playerId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _playerRepository.GetAsync(p => p.UserId == userId);
                if (player == null || player.Id != playerId)
                {
                    _response.Result = new { Message = "You don't have access to this content" };
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                var chatsList = _service.GetChatsForPlayer(player);

                _response.Result = new {ChatList = chatsList};
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet("chat/{receiverId:int}")]
        [Authorize] 
        public async Task<ActionResult<APIResponse>> GetWholeChatByReceiver(int receiverId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _playerRepository.GetAsync(p => p.UserId == userId);
                if(player == null)
                {
                    _response.Result = new { Message = "You don't have access to this content" };
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return Unauthorized(_response);
                }
                var wholeChat = await _service.GetWholeChatByReceiver(receiverId, player);
                _response.Result = wholeChat;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}
