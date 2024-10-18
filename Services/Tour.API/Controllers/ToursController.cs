using Microsoft.AspNetCore.Mvc;
using Tour.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

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

		[HttpGet]
		public async Task<IActionResult> GetAllToursAsync()
		{
			var response = await _tourService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetTourByIdAsync(int id)
		{
			var response = await _tourService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchTours([FromQuery] TourSearchRequestDTO request)
		{
			var fullResult = await _tourService.SearchToursAsync(request);
			return Ok(fullResult);
		}

		[HttpPost]
        [ApiValidationFilter]
        public async Task<IActionResult> CreateTourAsync(TourRequestDTO requestDTO)
        {
            var response = await _tourService.CreateAsync(requestDTO);
            return StatusCode(response.StatusCode, response);
        }

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
        public async Task<IActionResult> UpdateTourAsync(int id, TourRequestDTO requestDTO)
        {
            var response = await _tourService.UpdateAsync(id, requestDTO);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTourAsync(int id)
        {
            var response = await _tourService.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
