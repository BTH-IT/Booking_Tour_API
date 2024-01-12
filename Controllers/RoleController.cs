using BookingApi.DTO;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            this._roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _roleService.GetAll();

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _roleService.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpGet("role-detail/{id}")]
        public async Task<IActionResult> GetRoleDetailAllByRoleId(int id)
        {
            var data = await _roleService.GetRoleDetailAllByRoleId(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] RoleRequestDTO role)
        {
            var data = await _roleService.Insert(role);
            return StatusCode(data.ResponseCode, data);
        }

        [HttpPatch()]
        public async Task<IActionResult> Update([FromForm] RoleRequestDTO role)
        {
            var data = await _roleService.Update(role);
            return Ok(data);
        }

        [HttpPatch("role-detail")]
        public async Task<IActionResult> UpdateRoleDetailByRoleId([FromForm] RoleDetailDTO role)
        {
            var data = await _roleService.UpdateRoleDetailByRoleId(role);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _roleService.Delete(id);
            return StatusCode(data.ResponseCode, data);
        }
    }
}
