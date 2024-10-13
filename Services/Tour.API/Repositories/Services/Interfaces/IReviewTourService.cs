using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Repositories.Services.Interfaces
{
	public interface IReviewTourService
	{
		Task<ApiResponse<Review>> CreateReviewAsync(Review reviewRequest);
		Task<ApiResponse<Review>> UpdateReviewAsync(Review reviewRequest);
		Task<ApiResponse<int>> DeleteReviewAsync(int tourId, string reviewId);
	}
}
