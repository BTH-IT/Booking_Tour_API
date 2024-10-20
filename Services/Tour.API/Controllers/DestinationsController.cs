using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authorization;
using System.Security.Claims;
using Shared.Constants;
using Shared.Enums;
namespace Tour.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinationsController : Controller
    {
        private readonly IDestinationService _destinationService;

        public DestinationsController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _destinationService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _destinationService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [ApiValidationFilter]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> CreateDestinationAsync(DestinationRequestDTO requestDTO)
        {
            var response = await _destinationService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateDestinationAsync(int id, DestinationRequestDTO requestDTO)
        {
            var response = await _destinationService.UpdateAsync(id, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteDestinationAsync(int id)
        {
            var response = await _destinationService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
