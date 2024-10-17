using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using Saga.Orchestrator.API.GrpcClient.Protos;
namespace Booking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IdentityGrpcService.IdentityGrpcServiceClient _identityGrpcServiceClient;  
        private readonly RoomGrpcService.RoomGrpcServiceClient _roomGrpcServiceClient;
        private readonly TourGrpcService.TourGrpcServiceClient _tourGrpcServiceClient;
        public TestsController(IdentityGrpcService.IdentityGrpcServiceClient identityGrpcServiceClient, RoomGrpcService.RoomGrpcServiceClient roomGrpcServiceClient, TourGrpcService.TourGrpcServiceClient tourGrpcServiceClient)
        {
            _identityGrpcServiceClient = identityGrpcServiceClient;
            _roomGrpcServiceClient = roomGrpcServiceClient;
            _tourGrpcServiceClient = tourGrpcServiceClient;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserIdAsync(int userId)
        {
            try
            {
                var user = await _identityGrpcServiceClient.GetUserByIdAsync(new GetUserByIdRequest()
                {
                    Id = userId
                });
                if (user == null) throw new Exception();
                var response = new ApiResponse<object>(200,user, "Lấy dữ liệu người dùng thành công");
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex) 
            {
                var response = new ApiResponse<string>(200, null, "Có lỗi xảy ra");
                return StatusCode(response.StatusCode,response);
            }
        }
        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetRoomIdAsync(int roomId)
        {
            try
            {
                var room = await _roomGrpcServiceClient.GetRoomByIdAsync(new GetRoomByIdRequest()
                {
                    Id = roomId
                });
                if (room == null) throw new Exception();
                var response = new ApiResponse<object>(200, room, "Lấy dữ liệu người dùng thành công");
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>(200, null, "Có lỗi xảy ra");
                return StatusCode(response.StatusCode, response);
            }
        }
        [HttpGet("schedule/{scheduleId}")]
        public async Task<IActionResult> GetscheduleIdAsync(int scheduleId)
        {
            try
            {
                var schedule = await _tourGrpcServiceClient.GetScheduleByIdAsync(new GetScheduleByIdRequest()
                {
                    Id = scheduleId
                });
                if (schedule == null) throw new Exception();
                var response = new ApiResponse<object>(200, schedule, "Lấy dữ liệu người dùng thành công");
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>(200, null, "Có lỗi xảy ra");
                return StatusCode(response.StatusCode, response);
            }
        }
    }
}
