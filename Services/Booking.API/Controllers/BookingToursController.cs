using Booking.API.Services;
using Booking.API.Services.Interfaces;
using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Enums;
using Shared.Helper;
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
			return Ok();
        }
    }
}
