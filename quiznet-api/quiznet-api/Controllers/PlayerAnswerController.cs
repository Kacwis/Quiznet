using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Controllers
{
    [Route("api/playerAnswer")]
    [ApiController]
    public class PlayerAnswerController : ControllerBase
    {
        private readonly IPlayerAnswerRepository _repository;

        private readonly IMapper _mapper;

        protected APIResponse _response;

        public PlayerAnswerController(IPlayerAnswerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponse>> GetPlayerAnswerById(int id)
        {
            try
            {
                var playerAnswer = await _repository.GetAsync(a => a.Id == id);
                if(playerAnswer == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<string> { "There is no player answer with that id: " + id };
                    return NotFound(_response);
                }
                _response.Result = playerAnswer;
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



    }
}
