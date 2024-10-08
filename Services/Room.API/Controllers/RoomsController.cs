using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using FluentValidation;

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
		public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDTO requestDTO)
		{
			var response = await _roomService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateRoomAsync([FromBody] RoomRequestDTO requestDTO)
		{
			var response = await _roomService.UpdateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id:int}")]
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
