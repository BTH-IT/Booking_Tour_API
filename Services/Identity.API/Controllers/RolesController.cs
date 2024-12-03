using AutoMapper;
using Identity.API.Services;
using Identity.API.Services.Interfaces;
<<<<<<< HEAD
using Infrastructure.Authorization;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;
using Shared.Constants;
using ILogger = Serilog.ILogger;
using Shared.Enums;
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
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> CreateRoleAsync(RoleRequestDTO requestDTO)
        {
            var response = await _roleService.InsertAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{id:int}")]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            var response = await _roleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut]
        [ApiValidationFilter]
        [RoleRequirement(ERole.Admin)]

        public async Task<IActionResult> UpdateRoleAsync(RoleRequestDTO requestDTO)
        {
            var response = await _roleService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("{roleId}/role-details")]
        [ApiValidationFilter]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateRoleDetailForRole(string roleId,RoleDetailDTO requestDTO)
        {
            var response = await _roleService.UpdateRoleDetailByRoleIdAsync(roleId, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

    }
}
