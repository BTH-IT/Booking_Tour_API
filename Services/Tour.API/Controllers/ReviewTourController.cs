using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Tour.API.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ReviewTourController : ControllerBase
{
	private readonly IReviewTourService _reviewTourService;

	public ReviewTourController(IReviewTourService reviewTourService)
	{
		_reviewTourService = reviewTourService;
	}

	[HttpPost]
	public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewTourDTO reviewRequest)
	{
		var response = await _reviewTourService.CreateReviewAsync(reviewRequest);
		return StatusCode(response.StatusCode, response);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewTourDTO reviewRequest)
	{
		var response = await _reviewTourService.UpdateReviewAsync(reviewRequest);
		return StatusCode(response.StatusCode, response);
	}

	[HttpDelete("{tourId}/{reviewId}")]
	public async Task<IActionResult> DeleteReviewAsync(int tourId, string reviewId)
	{
		var response = await _reviewTourService.DeleteReviewAsync(tourId, reviewId);
		return StatusCode(response.StatusCode, response);
	}
}
