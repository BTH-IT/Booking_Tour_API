using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Shared.Constants;
using Shared.Enums;
namespace Room.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RoomsController : ControllerBase
	{
		private readonly IRoomService _roomService;

		public RoomsController(IRoomService roomService)
		{
			_roomService = roomService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var response = await _roomService.GetAllAsync();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetByIdAsync(int id)
		{
			var response = await _roomService.GetByIdAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[ApiValidationFilter]
		[Authorize]
        [RoleRequirement(ERole.Admin)]

        public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDTO requestDTO)
		{
			var response = await _roomService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
		[Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] RoomRequestDTO requestDTO)
		{
			var response = await _roomService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("{id:int}")]
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteRoomAsync(int id)
		{
			var response = await _roomService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("search")]
        public async Task<IActionResult> SearchRooms([FromQuery] RoomSearchRequestDTO request)
		{
			var rooms = await _roomService.SearchRoomsAsync(request);
			return Ok(rooms);
		}
	}
}
