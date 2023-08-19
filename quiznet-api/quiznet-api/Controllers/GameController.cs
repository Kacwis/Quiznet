using System.Net;
using AutoMapper;
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
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly IGameService _service;

        private readonly IPlayerRepository _playerRepository;

        protected APIResponse _response;

        private readonly IJwtHandler _jwtHandler;

        private readonly IMapper _mapper;

        public GameController(IGameService service, IJwtHandler jwtHandler, IMapper mapper, IPlayerRepository playerRepository)
        {
            _service = service;
            _response = new APIResponse();
            _jwtHandler = jwtHandler;
            _mapper = mapper;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllGames()
        {
            try
            {
                _response.Result = await _service.GetAllGameResponses();
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

        [HttpGet("randomGame/{playerId:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> StartGameWithRandomPlayer(int playerId)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var player = await _playerRepository.GetAsync(p => p.Id == playerId);
                if (player.User.Id != userId)
                {
                    _response.StatusCode = HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.Result = new { Message = "You don't have access to this content" };
                    return Ok(_response);
                }
                var createdGame = await _service.StartGameWithRandomPlayer(playerId);
                if (createdGame == null)
                {
                    _response.Result = new { Message = "Currently there are no available players to play with" };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    return Ok(_response);
                }
                _response.Result = new {GameId = createdGame.Id};
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

        [HttpGet("{gameId:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetGameById(int gameId)
        {
            try
            {
                var game = await _service.GetGameById(gameId);               
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);                
                if(game.Players.FirstOrDefault(p => p.User.Id == userId) == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "You don't have access to this content" };
                    return Unauthorized(_response);
                }
                _response.Result = await _service.GetGameResponseDTO(game);
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

        [HttpPost("{gameId:int}/round")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> AddRoundToGame([FromBody] CreateGameRoundDTO gameRoundDTO, int gameId)
        {
            try
            {
                var game = await _service.GetGameById(gameId);
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                if (game.Players.FirstOrDefault(p => p.User.Id == userId) == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "You don't have access to this content" };
                    return Unauthorized(_response);
                }
                _response.Result = await _service.AddRoundToGame(gameRoundDTO, gameId);
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

        [HttpDelete("{gameId:int}")]
        public async Task<ActionResult<APIResponse>> DeleteGameById(int gameId)
        {
            try
            {
                await _service.DeleteGameById(gameId);
                _response.Result = "Game deleted succesfully!";
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

        [HttpPut("round/{roundId:int}")]
        public async Task<ActionResult<APIResponse>> UpdateRound([FromBody] RoundUpdateDTO roundUpdateDTO, int roundId)
        {
            try
            {
                await _service.UpdateGameRound(roundId, roundUpdateDTO);
                _response.Result = "Game Round updated succesfully!";
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

        [HttpPost("friend")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> StartGameWithFriend([FromBody] CreateFriendGameDTO createFriendGameDto)
        {
            try
            {
                var userId = _jwtHandler.GetUserIdFromJwtToken(HttpContext);
                var createdGame  = await _service.StartGameWithFriend(createFriendGameDto);
                _response.Result = new { GameId = createdGame.Id };
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
    }

}
