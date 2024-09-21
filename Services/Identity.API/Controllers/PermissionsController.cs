using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionsController(IPermissionService permissionService) {
            this._permissionService = permissionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _permissionService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _permissionService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateRoleAsync(PermissionRequestDTO requestDTO)
        {
            var response = await _permissionService.InsertAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            var response = await _permissionService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateRoleAsync(PermissionRequestDTO requestDTO)
        {
            var response = await _permissionService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
