using Booking.API.Entities;
using Booking.API.Services.Interfaces;
using EventBus.IntergrationEvents.Events;
using Infrastructure.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Enums;
using Shared.Helper;
using System.Security.Claims;

namespace Booking.API.Controllers
{
	[ApiController]
	[Route("api/booking-serivce/[controller]")]
	public class BookingRoomsController : ControllerBase
	{
		private readonly IBookingRoomService _bookingRoomService;
		private readonly IPublishEndpoint _publishEndpoint;	
		public BookingRoomsController(IBookingRoomService bookingRoomService,
			IPublishEndpoint publishEndpoint)
		{
			_bookingRoomService = bookingRoomService;
			_publishEndpoint = publishEndpoint;
		}

		[HttpGet]
		[RoleRequirement(ERole.Admin)]
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

        [HttpGet("current-user")]
        public async Task<IActionResult> GetByCurrentUserAsync()
		{
			var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var response = await _bookingRoomService.GetCurrentUserAsync(int.Parse(userId));
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{roomId:int}/data")]
		public async Task<IActionResult> GetRoomCheckInCheckOutDataAsync(int roomId)
		{
			var response = await _bookingRoomService.GetRoomCheckInCheckOutDataAsync(roomId);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{roomId:int}/update-status")]
		[RoleRequirement(ERole.Admin)]
		public async Task<IActionResult> UpdateStatus(int roomId,UpdateBookingStatusDTO dto)
		{
			var response = await _bookingRoomService.GetRoomCheckInCheckOutDataAsync(roomId);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{bookingRoomId:int}/cancel")]
		public async Task<IActionResult> DeleteBookingRoomId(int bookingRoomId)
		{
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var response = await _bookingRoomService.DeleteBookingRoomIdAsync(bookingRoomId,int.Parse(userId!));
			return StatusCode(response.StatusCode, response);
        }
        [HttpPost("test-event")]
        public async Task<IActionResult> TestPublishEvent()
        {
            try
            {
                // Create a booking event for testing
                var newBookingRoom = new BookingRoomEvent()
                {
					Data = new BookingRoomResponseDTO(),
					Type = "Create",
                };

                // Publish the event to RabbitMQ
                await _publishEndpoint.Publish(newBookingRoom);

                return Ok(new ApiResponse<string>(200, "Event published successfully", "Success"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, null, $"Failed to publish event: {ex.Message}"));
            }
        }
    }
}
