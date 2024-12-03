<<<<<<< HEAD
﻿using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Shared.Constants;
using Shared.Enums;
=======
﻿using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
namespace Room.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HotelsController : ControllerBase
	{
		private readonly IHotelService _hotelService;

		public HotelsController(IHotelService hotelService)
		{
			_hotelService = hotelService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var response = await _hotelService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var response = await _hotelService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[ApiValidationFilter]
<<<<<<< HEAD
		[Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> CreateHotelAsync([FromBody] HotelRequestDTO requestDTO)
=======
		public async Task<IActionResult> CreateHotelAsync([FromBody] HotelRequestDTO requestDTO)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _hotelService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")] 
		[ApiValidationFilter]
<<<<<<< HEAD
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelRequestDTO requestDTO)
=======
		public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelRequestDTO requestDTO)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _hotelService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("{id:int}")]
<<<<<<< HEAD
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteHotelAsync(int id)
=======
		public async Task<IActionResult> DeleteHotelAsync(int id)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _hotelService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
