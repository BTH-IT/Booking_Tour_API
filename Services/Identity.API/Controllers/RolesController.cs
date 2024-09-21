using AutoMapper;
using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

using ILogger = Serilog.ILogger;
namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger _logger;
        public RolesController(IRoleService roleService,
            ILogger logger,
            IMapper mapper) { 
            this._roleService = roleService;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _roleService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _roleService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateRoleAsync(RoleRequestDTO requestDTO)
        {
            var response = await _roleService.InsertAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            var response = await _roleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateRoleAsync(RoleRequestDTO requestDTO)
        {
            var response = await _roleService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("{roleId}/role-details")]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateRoleDetailForRole(string roleId,RoleDetailDTO requestDTO)
        {
            var response = await _roleService.UpdateRoleDetailByRoleIdAsync(roleId, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

    }
}
