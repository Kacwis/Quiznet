using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;

namespace quiznet_api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        private readonly IMapper _mapper;

        protected APIResponse _response;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _response = new APIResponse();
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                var category = _mapper.Map<Category>(createCategoryDTO);
                var createdCategory = await _repository.CreateAsync(category);
                await _repository.SaveAsync();
                _response.Result = createdCategory;
                _response.StatusCode = System.Net.HttpStatusCode.Created;
                return Ok(_response);
            }
            catch( Exception ex)
            {
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllCategories()
        {
            try
            {
                var categories = await _repository.GetAllAsync();
                _response.Result = categories;
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


        [HttpGet("random")]
        public async Task<ActionResult<APIResponse>> GetRandomCategories()
        {
            try
            {
                var allCategories = await _repository.GetAllAsync();
                var random = new Random();
                List<Category> randomCategories = new List<Category>();
                while(randomCategories.Count < 3)
                {
                    int randomIndex = random.Next(allCategories.Count);
                    if (!randomCategories.Contains(allCategories[randomIndex])){
                        randomCategories.Add(allCategories[randomIndex]);
                    }
                }
                _response.Result = randomCategories;
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
