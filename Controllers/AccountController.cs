using BookingApi.DTO;
using BookingApi.Helpers;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _accountService.GetAll();

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _accountService.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] AccountRequestDTO account)
        {
            var data = await _accountService.Insert(account);
            return StatusCode(data.ResponseCode, data);
        }

        [HttpPatch()]
        public async Task<IActionResult> Update([FromForm] AccountRequestDTO account)
        {
            var data = await _accountService.Update(account);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _accountService.Delete(id);
            return StatusCode(data.ResponseCode, data);
        }
    }
}
