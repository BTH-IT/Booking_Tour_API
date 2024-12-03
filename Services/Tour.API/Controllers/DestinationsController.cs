<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authorization;
using System.Security.Claims;
using Shared.Constants;
using Shared.Enums;
=======
﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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

<<<<<<< HEAD
=======
        // Lấy tất cả các điểm đến
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _destinationService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
=======
        // Lấy một điểm đến theo ID
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _destinationService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
        [HttpPost]
        [ApiValidationFilter]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
=======
        // Tạo một điểm đến mới
        [HttpPost]
        [ApiValidationFilter]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> CreateDestinationAsync(DestinationRequestDTO requestDTO)
        {
            var response = await _destinationService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
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
=======
        // Cập nhật thông tin của một điểm đến
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateDestinationAsync(DestinationRequestDTO requestDTO)
        {
            var response = await _destinationService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa một điểm đến theo ID
        [HttpDelete("{id:int}")]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> DeleteDestinationAsync(int id)
        {
            var response = await _destinationService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
