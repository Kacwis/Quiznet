using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Services.IServices;

namespace quiznet_api.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly IGameService _service;

        protected APIResponse _response;

        public GameController(IGameService service)
        {
            _service = service;
            _response = new APIResponse();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllGames()
        {
            try
            {
                _response.Result = await _service.GetAllGames();
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
        public async Task<ActionResult<APIResponse>> StartGameWithRandomPlayer(int playerId)
        {
            try
            {
                var createdGame = await _service.StartGameWithRandomPlayer(playerId);
                _response.Result = createdGame.Id;
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
        public async Task<ActionResult<APIResponse>> GetGameById(int gameId)
        {
            try
            {
                var game = await _service.GetGameById(gameId);
                _response.Result = game;
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

        [HttpGet("player/{playerId:int}")]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetActiveGamesByPlayerId(int playerId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString();
                System.Diagnostics.Debug.WriteLine(token);
                _response.Result = await _service.GetActiveGamesByPlayerId(playerId);
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
        public async Task<ActionResult<APIResponse>> AddRoundToGame([FromBody] CreateGameRoundDTO gameRoundDTO, int gameId)
        {
            try
            {
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

    }

}
