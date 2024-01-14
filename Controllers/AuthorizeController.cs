using BookingApi.DTO;
using BookingApi.Services;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] AuthLoginDTO login)
        {
            var data = await _authService.Login(login);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] AuthRegisterDTO register)
        {
            var data = await _authService.Register(register);

            if (!data)
            {
                return BadRequest();
            }

            return Ok(data);
        }

        [HttpGet("refresh/{refreshToken}")]
        public async Task<IActionResult> refresh(string refreshToken)
        {
            var data = await _authService.RefreshToken(refreshToken);

            if (data == string.Empty)
            {
                return Unauthorized();
            }

            return Ok(data);
        }
    }
}
