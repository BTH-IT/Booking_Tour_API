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
		private readonly IValidator<RoomRequestDTO> _roomValidator;

		public RoomsController(IRoomService roomService, IValidator<RoomRequestDTO> roomValidator)
		{
			_roomService = roomService;
			_roomValidator = roomValidator;
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

		// Tạo một phòng mới
		[HttpPost]
		[ApiValidationFilter]
		public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDTO requestDTO)
		{
			var validationResult = await _roomValidator.ValidateAsync(requestDTO);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var response = await _roomService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		[ApiValidationFilter]
		public async Task<IActionResult> UpdateRoomAsync([FromBody] RoomRequestDTO requestDTO)
		{
			var validationResult = await _roomValidator.ValidateAsync(requestDTO);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var response = await _roomService.UpdateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteRoomAsync(int id)
		{
			var response = await _roomService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
