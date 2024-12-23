﻿using Microsoft.AspNetCore.Mvc;
using Booking.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Authorization;
using Shared.Enums;
using System.Security.Claims;

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
    }
}
