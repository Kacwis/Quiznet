using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiznet_api.Models;
using quiznet_api.Models.DTO;
using quiznet_api.Repository.IRepository;
using quiznet_api.Services.IServices;
using System.Diagnostics;

namespace quiznet_api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        
        public AuthController(IUserService service)
        {
            _service = service;            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {            
            var loginResponse = await _service.Login(loginRequestDTO);
            if (loginResponse == null)
            {
                return NotFound();
            }
            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            if(!_service.IsUniqueUser(registrationRequestDTO.Username))
            {
                return BadRequest("This username is already in use!");
            }
            await _service.RegisterUser(registrationRequestDTO);
            return Ok("User registred succesfully");
        }
    }
}
