using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Services.Interfaces
{
	public interface IReviewHotelService
	{
		Task<ApiResponse<ReviewHotelDTO>> CreateReviewAsync(ReviewHotelDTO reviewRequest);
		Task<ApiResponse<ReviewHotelDTO>> UpdateReviewAsync(ReviewHotelDTO reviewRequest);
		Task<ApiResponse<int>> DeleteReviewAsync(int hotelId, string reviewId);
	}
}
