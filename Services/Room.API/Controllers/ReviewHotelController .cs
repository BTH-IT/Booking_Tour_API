using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Room.API.Services.Interfaces;
using Shared.DTOs;

namespace Room.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class ReviewHotelController : ControllerBase
	{
		private readonly IReviewHotelService _reviewHotelService;

		public ReviewHotelController(IReviewHotelService reviewHotelService)
		{
			_reviewHotelService = reviewHotelService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewHotelDTO reviewRequest)
		{
			var response = await _reviewHotelService.CreateReviewAsync(reviewRequest);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewHotelDTO reviewRequest)
		{
		var response = await _reviewHotelService.UpdateReviewAsync(reviewRequest);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{hotelId}/{reviewId}")]
		public async Task<IActionResult> DeleteReviewAsync(int hotelId, string reviewId)
		{
			var response = await _reviewHotelService.DeleteReviewAsync(hotelId, reviewId);
			return StatusCode(response.StatusCode, response);
		}
	} 
}