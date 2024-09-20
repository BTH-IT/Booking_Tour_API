using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using FluentValidation;

namespace Room.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HotelsController : ControllerBase
	{
		private readonly IHotelService _hotelService;
		private readonly IValidator<HotelRequestDTO> _hotelValidator;

		public HotelsController(IHotelService hotelService, IValidator<HotelRequestDTO> hotelValidator)
		{
			_hotelService = hotelService;
			_hotelValidator = hotelValidator;
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
			var validationResult = await _hotelValidator.ValidateAsync(requestDTO);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var response = await _hotelService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateHotelAsync([FromBody] HotelRequestDTO requestDTO)
		{
			var validationResult = await _hotelValidator.ValidateAsync(requestDTO);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var response = await _hotelService.UpdateAsync(requestDTO);
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
