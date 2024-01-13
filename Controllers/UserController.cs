using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<UserResponseDTO> users = await _userService.GetAll(); // Await the asynchronous task

                if (users != null)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound(); // Return NotFound if no data is found
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(500, "Internal Server Error"); // Return a generic 500 status code for exceptions
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _userService.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserRequestDTO user)
        {
            var data = await _userService.Insert(user);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPatch()]
        public async Task<IActionResult> Update([FromForm] UserRequestDTO user)
        {
            var data = await _userService.Update(user);

            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _userService.Delete(id);

            return StatusCode(data.StatusCode, data);
        }
    }
}
