using Booking.API.GrpcServer.Protos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.BookingRoomOrderManagers;
using Shared.DTOs;
using Shared.Helper;
using System.Security.Claims;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SagasController : ControllerBase
    {
        private readonly BookingRoomManager _bookingRoomManager;
        public SagasController(BookingRoomManager bookingRoomManager)
        {
            _bookingRoomManager = bookingRoomManager;   
        }
        [HttpPost("booking-room")]
        public async Task<IActionResult> CreateBookingRoomAsync(CreateBookingRoomOrderDto request)
        {
            if(request.CheckIn == null || request.CheckOut == null)
            {
                var error = new ApiResponse<int>(400,-1,"Vui lòng không bỏ trống ngày check in và check out");

                return StatusCode(error.StatusCode, error);
            }    
            var response = await _bookingRoomManager.CreateBookingRoomOrder(request);
            return StatusCode(response.StatusCode, response);
        }
    }
}
