using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;

namespace quiznet_api.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private readonly IPlayerRepository _repository;

        protected APIResponse _response;

        private readonly IMapper _mapper;

        public PlayerController(IPlayerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _response = new APIResponse();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllPlayers()
        {
            try
            {                
                _response.Result = await _repository.GetAllAsync();
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
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
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "user_id").Value;
                System.Diagnostics.Debug.WriteLine(userId + " " + player.User.Id + id);
                if (userId != player.User.Id.ToString())
                {
                    _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    _response.ErrorMessages = new List<string>() { "You don't have access to this content" };
                    _response.IsSuccess = false;
                    return Unauthorized(_response);
                }
                var menuResponseDTO = GetMenuResponseDTO(player);
                _response.Result = menuResponseDTO;
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
        
        private MenuResponseDTO GetMenuResponseDTO(Player player)
        {
            var activeGames = new List<GameResponseDTO>();
            var finishedGames = new List<GameResponseDTO>();
            foreach(var game in player.Games)
            {
                var currentGameResponse = _mapper.Map<GameResponseDTO>(game);
                if(game.Status == "IN_PROGRESS")
                {
                    activeGames.Add(currentGameResponse);
                }
                else
                {
                    finishedGames.Add(currentGameResponse);
                }                
            }
            return new MenuResponseDTO()
            {
                ActiveGames = activeGames,
                FinishedGames = finishedGames
            };
        }

    }
}
