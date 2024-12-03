<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;
namespace Room.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ReviewRoomController : ControllerBase
	{
		private readonly IReviewRoomService _ReviewRoomService;

		public ReviewRoomController(IReviewRoomService ReviewRoomService)
		{
			_ReviewRoomService = ReviewRoomService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewRoomDTO reviewRequest)
		{
			var response = await _ReviewRoomService.CreateReviewAsync(reviewRequest);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewRoomDTO reviewRequest)
		{
			var response = await _ReviewRoomService.UpdateReviewAsync(reviewRequest);
				return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{roomId}/{reviewId}")]
		public async Task<IActionResult> DeleteReviewAsync(int roomId, string reviewId)
		{
			var response = await _ReviewRoomService.DeleteReviewAsync(roomId, reviewId);
			return StatusCode(response.StatusCode, response);
		}
=======
﻿using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ReviewRoomController : ControllerBase
{
	private readonly IReviewRoomService _ReviewRoomService;

	public ReviewRoomController(IReviewRoomService ReviewRoomService)
	{
		_ReviewRoomService = ReviewRoomService;
	}

	[HttpPost]
	public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewRoomDTO reviewRequest)
	{
		var response = await _ReviewRoomService.CreateReviewAsync(reviewRequest);
		return StatusCode(response.StatusCode, response);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewRoomDTO reviewRequest)
	{
		var response = await _ReviewRoomService.UpdateReviewAsync(reviewRequest);
		return StatusCode(response.StatusCode, response);
	}

	[HttpDelete("{roomId}/{reviewId}")]
	public async Task<IActionResult> DeleteReviewAsync(int roomId, string reviewId)
	{
		var response = await _ReviewRoomService.DeleteReviewAsync(roomId, reviewId);
		return StatusCode(response.StatusCode, response);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}