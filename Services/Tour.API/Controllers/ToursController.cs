<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authorization;
using Shared.Enums;
=======
﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using System.Threading.Tasks;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

namespace Tour.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToursController : Controller
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

<<<<<<< HEAD
		[HttpGet]
		public async Task<IActionResult> GetAllToursAsync()
		{
			var response = await _tourService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetTourByIdAsync(int id)
		{
			var response = await _tourService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchTours([FromQuery] TourSearchRequestDTO request)
		{
			var fullResult = await _tourService.SearchToursAsync(request);
			return Ok(fullResult);
		}

		[HttpPost]
        [ApiValidationFilter]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
=======
        // Tạo một tour mới
        [HttpPost]
        [ApiValidationFilter]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> CreateTourAsync(TourRequestDTO requestDTO)
        {
            var response = await _tourService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
		[HttpPut("{id:int}")]
		[ApiValidationFilter]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateTourAsync(int id, TourRequestDTO requestDTO)
        {
            var response = await _tourService.UpdateAsync(id, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
=======
        // Cập nhật thông tin của một tour
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateTourAsync(TourRequestDTO requestDTO)
        {
            var response = await _tourService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Lấy tất cả các tour
        [HttpGet]
        public async Task<IActionResult> GetAllToursAsync()
        {
            var response = await _tourService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Lấy tour theo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTourByIdAsync(int id)
        {
            var response = await _tourService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa tour theo ID
        [HttpDelete("{id:int}")]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> DeleteTourAsync(int id)
        {
            var response = await _tourService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
<<<<<<< HEAD
=======

        [HttpGet("search")]
        public async Task<IActionResult> SearchTours([FromQuery] TourSearchRequestDTO request)
        {
                var fullResult = await _tourService.SearchToursAsync(request);
                return Ok(fullResult);
        }

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
}
