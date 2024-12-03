<<<<<<< HEAD
﻿using Booking.API.Services.Interfaces;
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
=======
﻿using AutoMapper;
using Booking.API.GrpcClient.Protos;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Helper;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingToursController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        public BookingToursController(
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient,
            RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient,
            TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient) 
        {
            this._mapper = mapper;
            this._publishEndpoint = publishEndpoint;
            this._identityGrpcServiceClient = identityGrpcServiceClient;
            this._roomGrpcServiceClient = roomGrpcServiceClient;
            this._tourGrpcServiceClient = tourGrpcServiceClient;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOutBookingToursAsync([FromBody] BookingTourRequestDTO requestDto)
        {
            await _publishEndpoint.Publish<TestEvent>(new TestEvent
            {
                Hello = "hello ne"
            });
            return Ok();
        }
        [HttpGet("test-grpc")]
        public async Task<IActionResult> TestGrpcAsync()
        {
            var test1 = await _identityGrpcServiceClient.GetUserByIdAsync(new GetUserByIdRequest
            {
                Id = 10
            });
            var test2 = await _roomGrpcServiceClient.GetRoomByIdAsync(new GetRoomByIdRequest
            {
                Id = 11
            });
            var test3 = await _tourGrpcServiceClient.GetTourByIdAsync(new GetTourByIdRequest
            {
                Id = 12
            });
            return Ok(new
            {
                test1,
                test2,
                test3
            });
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        }
    }
}
