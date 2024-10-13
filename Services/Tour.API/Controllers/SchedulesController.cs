using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulesController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Lấy tất cả các lịch trình
        [HttpGet]
        public async Task<IActionResult> GetAllSchedulesAsync()
        {
            var response = await _scheduleService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Lấy một lịch trình theo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetScheduleByIdAsync(int id)
        {
            var response = await _scheduleService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

		[HttpGet("tour/{tourId:int}")]
		public async Task<IActionResult> GetScheduleByTourIdAsync(int tourId)
		{
			var response = await _scheduleService.GetByTourIdAsync(tourId);
			return StatusCode(response.StatusCode, response);
		}

		// Tạo một lịch trình mới
		[HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateScheduleAsync(ScheduleRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest("Thông tin lịch trình không hợp lệ.");
            }

            var response = await _scheduleService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Cập nhật thông tin của một lịch trình
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateScheduleAsync(ScheduleRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest("Thông tin lịch trình không hợp lệ.");
            }

            var response = await _scheduleService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa một lịch trình theo ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            var response = await _scheduleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }

}
