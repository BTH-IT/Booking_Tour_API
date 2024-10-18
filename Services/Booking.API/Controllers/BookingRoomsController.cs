using Microsoft.AspNetCore.Mvc;
using Booking.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BookingRoomsController : ControllerBase
	{
		private readonly IBookingRoomService _bookingRoomService;

		public BookingRoomsController(IBookingRoomService bookingRoomService)
		{
			_bookingRoomService = bookingRoomService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var response = await _bookingRoomService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var response = await _bookingRoomService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[ApiValidationFilter]
		public async Task<IActionResult> CreateBookingRoomAsync([FromBody] BookingRoomRequestDTO requestDTO)
		{
			var response = await _bookingRoomService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateBookingRoomAsync(int id, [FromBody] BookingRoomRequestDTO requestDTO)
		{
			var response = await _bookingRoomService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteBookingRoomAsync(int id)
		{
			var response = await _bookingRoomService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
