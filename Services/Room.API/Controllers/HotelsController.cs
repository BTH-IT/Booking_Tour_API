using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

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
		public async Task<IActionResult> CreateHotelAsync([FromBody] HotelRequestDTO requestDTO)
		{
			var response = await _hotelService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")] 
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateHotelAsync(int id, [FromBody] HotelRequestDTO requestDTO)
		{
			var response = await _hotelService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteHotelAsync(int id)
		{
			var response = await _hotelService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
