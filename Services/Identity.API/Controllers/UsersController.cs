using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) { 
            this._userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateUserAsync(UserRequestDTO requestDTO)
        {
            var response = await _userService.InsertAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var response = await _userService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("{id:int}")]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateUserAsync(int id,UpdateUserRequestDTO requestDTO)
        {
            var response = await _userService.UpdateAsync(id,requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> PutUserPasswordAsync([FromBody] ChangeUserPasswordRequestDto request)
        {
            var currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.PrimarySid)!.ToString());
            var response = await _userService.ChanageUserPasswordAsync(currentUserId, request);
            return StatusCode(response.StatusCode, response);
        }
    }
}
