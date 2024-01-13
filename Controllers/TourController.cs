using Microsoft.AspNetCore.Mvc;
using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Interfaces;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tourService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tourService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TourRequestDTO item)
        {
            var result = await _tourService.Insert(item);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TourRequestDTO item)
        {
            var result = await _tourService.Update(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tourService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
