using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Model;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using System.Net;

namespace quiznet_api.Controllers
{
    [Route("api/dictionary")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        protected APIResponse _response;

        private readonly IWordRepository _repository;

        private readonly IMapper _mapper;

        public DictionaryController(IWordRepository repository, IMapper mapper)
        {
            _response = new APIResponse();   
            _repository = repository;
            _mapper = mapper;

        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateWordTranslation([FromBody] WordCreateDTO wordCreateDTO)
        {
            try
            {
                if (await _repository.GetAsync(wordTranslation => wordTranslation.Word == wordCreateDTO.Word) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "There is already exercise with that name " + wordCreateDTO.Word };
                    return BadRequest(_response);
                }

                var mappedWord = _mapper.Map<WordTranslation>(wordCreateDTO);

                await _repository.CreateAsync(mappedWord);
                await _repository.SaveAsync();

                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message }; 
            }
            return _response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllWords()
        {
            try
            {                
                _response.Result = await _repository.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;                
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.Message };
            }
            return _response;
        }



    }
}
