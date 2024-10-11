using Shared.DTOs;
using Shared.Helper;

namespace Room.API.Services.Interfaces
{
	public interface IReviewRoomService
	{
		Task<ApiResponse<ReviewRoomDTO>> CreateReviewAsync(ReviewRoomDTO reviewRequest);
		Task<ApiResponse<ReviewRoomDTO>> UpdateReviewAsync(ReviewRoomDTO reviewRequest);
		Task<ApiResponse<int>> DeleteReviewAsync(int roomId, string reviewId);
	}
}