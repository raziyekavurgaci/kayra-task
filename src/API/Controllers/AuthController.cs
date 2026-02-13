using Microsoft.AspNetCore.Mvc;
using Application.Commands.Auth;
using Application.DTOs.Auth;
using Application.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDto>> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterCommand { RegisterDto = dto };
            var token = await _authService.HandleAsync(command);
            
            return Ok(token);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
        {
            var command = new LoginCommand { LoginDto = dto };
            var token = await _authService.HandleAsync(command);
            
            return Ok(token);
        }
    }
}
