<<<<<<< HEAD
﻿using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using Shared.Constants;
using Shared.Enums;
=======
﻿using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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
<<<<<<< HEAD
		[Authorize]
        [RoleRequirement(ERole.Admin)]

        public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDTO requestDTO)
=======
		public async Task<IActionResult> CreateRoomAsync([FromBody] RoomRequestDTO requestDTO)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _roomService.CreateAsync(requestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id:int}")]
		[ApiValidationFilter]
<<<<<<< HEAD
		[Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] RoomRequestDTO requestDTO)
=======
		public async Task<IActionResult> UpdateRoomAsync(int id, [FromBody] RoomRequestDTO requestDTO)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _roomService.UpdateAsync(id, requestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("{id:int}")]
<<<<<<< HEAD
        [Authorize]
        [RoleRequirement(ERole.Admin)]
        public async Task<IActionResult> DeleteRoomAsync(int id)
=======
		public async Task<IActionResult> DeleteRoomAsync(int id)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var response = await _roomService.DeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("search")]
<<<<<<< HEAD
        public async Task<IActionResult> SearchRooms([FromQuery] RoomSearchRequestDTO request)
=======
		public async Task<IActionResult> SearchRooms([FromQuery] RoomSearchRequestDTO request)
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		{
			var rooms = await _roomService.SearchRoomsAsync(request);
			return Ok(rooms);
		}
	}
}
