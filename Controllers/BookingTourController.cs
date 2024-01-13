using Microsoft.AspNetCore.Mvc;
using BookingApi.DTO;
using BookingApi.Services.Interfaces;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingTourController : ControllerBase
    {
        private readonly IBookingTourService _bookingTourService;

        public BookingTourController(IBookingTourService bookingTourService)
        {
            _bookingTourService = bookingTourService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookingTourService.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingTourService.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] BookingTourRequestDTO item)
        {
            var result = await _bookingTourService.Insert(item);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingTourRequestDTO item)
        {
            var result = await _bookingTourService.Update(item);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookingTourService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
