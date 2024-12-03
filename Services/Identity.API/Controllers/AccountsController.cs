using Identity.API.Services.Interfaces;
<<<<<<< HEAD
using Infrastructure.Authorization;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.Enums;
using Shared.Helper;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _accountService.GetAllAsync();
            return StatusCode(response.StatusCode,response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _accountService.GetByIdAsync(id);  
            return StatusCode(response.StatusCode,response);
        }
        [HttpPost]
        [ApiValidationFilter]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> CreateAccountAsync(AccountRequestDTO requestDTO)
        {
            var response = await _accountService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode,response);    
        }
        [HttpDelete("{id:int}")]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteAccountAsync(int id)
        {
            var response = await _accountService.DeleteAsync(id);
            return StatusCode(response.StatusCode,response);
        }
        [HttpPut]
        [ApiValidationFilter]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateAccountAsync(AccountRequestDTO requestDTO)
        {
            var response = await _accountService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        
    }
}
