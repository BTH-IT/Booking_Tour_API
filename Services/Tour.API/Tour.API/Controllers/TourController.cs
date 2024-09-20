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
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly IValidator<TourRequestDTO> _tourValidator;

        public TourController(ITourService tourService, IValidator<TourRequestDTO> tourValidator)
        {
            _tourService = tourService;
            _tourValidator = tourValidator;
        }

        // Tạo một tour mới
        [HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateTourAsync(TourRequestDTO requestDTO)
        {
            var validationResult = await _tourValidator.ValidateAsync(requestDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _tourService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        // Cập nhật thông tin của một tour
        [HttpPut]
        [ApiValidationFilter]
        public async Task<IActionResult> UpdateTourAsync(TourRequestDTO requestDTO)
        {
            var validationResult = await _tourValidator.ValidateAsync(requestDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
    }
}
