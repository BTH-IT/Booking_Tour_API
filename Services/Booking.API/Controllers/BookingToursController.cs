using Booking.API.Services.Interfaces;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Enums;
using System.Security.Claims;

namespace Booking.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
    [Authorize]

    public class BookingToursController : ControllerBase
	{
		private readonly IBookingTourService _bookingTourService;

		public BookingToursController(IBookingTourService bookingTourService)
		{
			_bookingTourService = bookingTourService;
		}

		[HttpGet]
		[RoleRequirement(ERole.Admin)]
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
		[HttpGet("current-user")]
		public async Task<IActionResult> GetByCurrentUserAsync()
		{
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var response = await _bookingTourService.GetCurrentUserAsync(int.Parse(userId!));
			return StatusCode(response.StatusCode,response);
		}
		[HttpPatch("{bookingTourId:int}/update-info")]
		public async Task<IActionResult> UpdateBookingTourAsync(int bookingTourId,UpdateBookingTourInfoRequest requestDtO)
		{
			var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var userRole = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Role)!);

			var response = await _bookingTourService.UpdateBookingTourInfoAsync(bookingTourId, requestDtO, userId, userRole);
			return StatusCode(response.StatusCode, response);			
		}
		[HttpDelete("{bookingTourId:int}/cancel")]
		public async Task<IActionResult> DeleteBookingTourAsync(int bookingTourId)
		{
			var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var response = await _bookingTourService.DeleteBookingTourAsync(bookingTourId,userId);
			return StatusCode(response.StatusCode, response);
        }
    }
}
