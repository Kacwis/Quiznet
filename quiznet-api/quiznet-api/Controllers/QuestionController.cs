using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;

namespace quiznet_api.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        protected APIResponse _response;

        public QuestionController(IQuestionService service)
        {
            _service = service;
            _response = new APIResponse();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateQuestion([FromBody] CreateQuestionDTO createQuestionDTO)
        {
            try
            {
                _response.Result = await _service.CreateQuestion(createQuestionDTO);
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

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllQuestions()
        {
            try
            {
                _response.Result = await _service.GetAllQuestions();
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

        [HttpDelete("{questionId:int}")]
        public async Task<ActionResult<APIResponse>> DeleteQuestion(int questionId)
        {
            try
            {
                await _service.DeleteQuestionById(questionId);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = "Question deleted succesfully";
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

        [HttpGet("{categoryName}")]
        public async Task<ActionResult<APIResponse>> GetQuestionsByCategory(string categoryName)
        {
            try
            {
                var questions = await _service.GetQuestionsByCategory(categoryName);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = questions;
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

        [HttpGet("random/{questionsNumber:int}/by/{categoryName}")]
        public async Task<ActionResult<APIResponse>> GetRandomQuestionsByCategory(int questionsNumber, string categoryName)
        {
            try
            {
                _response.Result = await _service.GetRandomQuestionsByCategory(questionsNumber, categoryName);
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
