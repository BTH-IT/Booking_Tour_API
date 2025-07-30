using Booking.API.GrpcServer.Protos;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Saga.Orchestrator.BookingRoomOrderManagers;
using Saga.Orchestrator.BookingTourOrderManagers;
using Shared.DTOs;
using Shared.Helper;
using System.Security.Claims;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/saga-service/[controller]")]
    [ApiController]
    //[Authorize]
    public class SagasController : ControllerBase
    {
        private readonly BookingRoomManager _bookingRoomManager;
        private readonly BookingTourManager _bookingTourManager;
        private IConfiguration _configuration;
        public SagasController(BookingRoomManager bookingRoomManager,
            BookingTourManager bookingTourManager,
            IConfiguration configuration
            )
        {
            _bookingRoomManager = bookingRoomManager; 
            _bookingTourManager = bookingTourManager; 
            _configuration = configuration;
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
        [HttpPost("booking-tour")]
        public async Task<IActionResult> CreateBookingTourAsync(CreateBookingTourOrderDto request)
        {
            var response = await  _bookingTourManager.CreateBookingTourOrder(request);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return Ok(new {
                tmp = _configuration.GetSection("SMTPEmailSettings:Password")
            });
        }
    
    }
}
