using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

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

<<<<<<< HEAD
=======
        // Lấy tất cả các lịch trình
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        [HttpGet]
        public async Task<IActionResult> GetAllSchedulesAsync()
        {
            var response = await _scheduleService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
=======
        // Lấy một lịch trình theo ID
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetScheduleByIdAsync(int id)
        {
            var response = await _scheduleService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
		[HttpGet("tour/{tourId:int}")]
		public async Task<IActionResult> GetScheduleByTourIdAsync(int tourId)
		{
			var response = await _scheduleService.GetByTourIdAsync(tourId);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
        [ApiValidationFilter]
        [Authorize]
=======
        // Tạo một lịch trình mới
        [HttpPost]
        [ApiValidationFilter]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> CreateScheduleAsync(ScheduleRequestDTO requestDTO)
        {
            if (requestDTO == null)
            {
                return BadRequest("Thông tin lịch trình không hợp lệ.");
            }

            var response = await _scheduleService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

<<<<<<< HEAD
		[HttpPut("{id:int}")]
		[ApiValidationFilter]
        [Authorize]
        public async Task<IActionResult> UpdateScheduleAsync(int id, ScheduleRequestDTO requestDTO)
=======
        // Cập nhật thông tin của một lịch trình
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateScheduleAsync(ScheduleRequestDTO requestDTO)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        {
            if (requestDTO == null)
            {
                return BadRequest("Thông tin lịch trình không hợp lệ.");
            }

<<<<<<< HEAD
            var response = await _scheduleService.UpdateAsync(id, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
=======
            var response = await _scheduleService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa một lịch trình theo ID
        [HttpDelete("{id:int}")]
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task<IActionResult> DeleteScheduleAsync(int id)
        {
            var response = await _scheduleService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
<<<<<<< HEAD
=======

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
