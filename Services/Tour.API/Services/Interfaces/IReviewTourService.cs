using Shared.DTOs;
using Shared.Helper;

namespace Tour.API.Services.Interfaces
{
    public interface IReviewTourService
    {
        Task<ApiResponse<ReviewTourDTO>> CreateReviewAsync(ReviewTourDTO reviewRequest);
        Task<ApiResponse<ReviewTourDTO>> UpdateReviewAsync(ReviewTourDTO reviewRequest);
        Task<ApiResponse<int>> DeleteReviewAsync(int tourId, string reviewId);
    }
}
