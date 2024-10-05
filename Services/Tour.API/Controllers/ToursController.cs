using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using System.Threading.Tasks;

namespace Tour.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToursController : Controller
    {
        private readonly ITourService _tourService;

        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // Tạo một tour mới
        [HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateTourAsync(TourRequestDTO requestDTO)
        {
            var response = await _tourService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Cập nhật thông tin của một tour
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateTourAsync(TourRequestDTO requestDTO)
        {
            var response = await _tourService.UpdateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Lấy tất cả các tour
        [HttpGet]
        public async Task<IActionResult> GetAllToursAsync()
        {
            var response = await _tourService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Lấy tour theo ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTourByIdAsync(int id)
        {
            var response = await _tourService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // Xóa tour theo ID
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTourAsync(int id)
        {
            var response = await _tourService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTours([FromQuery] TourSearchRequestDTO request)
        {
            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                var paginatedResult = await _tourService.SearchToursWithPaginationAsync(request);
                return Ok(paginatedResult);
            }
            else
            {
                var fullResult = await _tourService.SearchToursAsync(request);
                return Ok(fullResult);
            }
        }

    }
}
