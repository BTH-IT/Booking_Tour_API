using AutoMapper;
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

        [HttpPost("booking")]
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
        }
    }
}
