using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizesController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthorizesController(IAuthService authService)
        {
            this._authService = authService;
        }
        [HttpPost("login")]
        [ApiValidationFilter]
        public async Task<IActionResult> login([FromBody] AuthLoginDTO login)
        {
            var response = await _authService.LoginAsync(login);
            return StatusCode(response.StatusCode,response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] AuthRegisterDTO register)
        {
            var response = await _authService.RegisterAsync(register);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> refresh(string refreshToken)
        {
            var response = await _authService.RefreshToken(refreshToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
