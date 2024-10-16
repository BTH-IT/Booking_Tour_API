using Booking.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookingToursController : ControllerBase
	{
		private readonly IBookingTourService _bookingTourService;

		public BookingToursController(IBookingTourService bookingTourService)
		{
			_bookingTourService = bookingTourService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var response = await _bookingTourService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var response = await _bookingTourService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[ApiValidationFilter]
		public async Task<IActionResult> CreateBookingTourAsync([FromBody] BookingTourRequestDTO requestDTO)
		{
			var response = await _bookingTourService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateBookingTourAsync(int id, [FromBody] BookingTourRequestDTO requestDTO)
		{
			var response = await _bookingTourService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteBookingTourAsync(int id)
		{
			var response = await _bookingTourService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
