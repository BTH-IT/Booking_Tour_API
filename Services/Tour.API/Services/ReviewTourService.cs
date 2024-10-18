using AutoMapper;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Repositories.Interfaces;
using ReviewEntity = Tour.API.Entities.Review;
using ILogger = Serilog.ILogger;
using Tour.API.Services.Interfaces;

public class ReviewTourService : IReviewTourService
{
	private readonly ITourRepository _TourRepository;
	private readonly IMapper _mapper;
	private readonly ILogger _logger;

	public ReviewTourService(ITourRepository TourRepository, ILogger logger, IMapper mapper)
	{
		_TourRepository = TourRepository;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<ApiResponse<ReviewTourDTO>> CreateReviewAsync(ReviewTourDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewTourService - CreateReviewAsync");
		try
		{
			var tour = await _TourRepository.GetTourByIdAsync(reviewRequest.TourId);
			if (tour == null)
			{
				_logger.Information("Tour not found.");
				return new ApiResponse<ReviewTourDTO>(404, null, "Tour not found.");
			}

			var reviewEntity = _mapper.Map<ReviewEntity>(reviewRequest);
			reviewEntity.CreatedAt = DateTime.UtcNow;

			if (tour.ReviewList == null)
			{
				tour.ReviewList = new List<ReviewEntity>();
			}

			tour.ReviewList.Add(reviewEntity);

			await _TourRepository.UpdateTourAsync(tour);

			var responseData = _mapper.Map<ReviewTourDTO>(reviewEntity);
			_logger.Information("End: ReviewTourService - CreateReviewAsync");
			return new ApiResponse<ReviewTourDTO>(200, responseData, "Review created successfully.");
		}
		catch (Exception ex)
		{
			_logger.Error($"Error in ReviewTourService - CreateReviewAsync: {ex.Message}", ex);
			return new ApiResponse<ReviewTourDTO>(500, null, $"An error occurred while creating the review: {ex.Message}");
		}
	}

	public async Task<ApiResponse<ReviewTourDTO>> UpdateReviewAsync(ReviewTourDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewTourService - UpdateReviewAsync");
		try
		{
			var tour = await _TourRepository.GetTourByIdAsync(reviewRequest.TourId);
			if (tour == null)
			{
				_logger.Information("Tour not found.");
				return new ApiResponse<ReviewTourDTO>(404, null, "Tour not found.");
			}

			var review = tour.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
			if (review == null)
			{
				_logger.Information("Review not found.");
				return new ApiResponse<ReviewTourDTO>(404, null, "Review not found.");
			}

			review.Content = reviewRequest.Content;
			review.Rating = reviewRequest.Rating;
			review.UpdatedAt = DateTime.UtcNow;

			await _TourRepository.UpdateTourAsync(tour);

			var responseData = _mapper.Map<ReviewTourDTO>(review);
			_logger.Information("End: ReviewTourService - UpdateReviewAsync");
			return new ApiResponse<ReviewTourDTO>(200, responseData, "Review updated successfully.");
		}
		catch (Exception ex)
		{
			_logger.Error($"Error in ReviewTourService - UpdateReviewAsync: {ex.Message}", ex);
			return new ApiResponse<ReviewTourDTO>(500, null, $"An error occurred while updating the review: {ex.Message}");
		}
	}

	public async Task<ApiResponse<int>> DeleteReviewAsync(int TourId, string reviewId)
	{
		_logger.Information($"Begin: ReviewTourService - DeleteReviewAsync : {reviewId}");
		try
		{
			var tour = await _TourRepository.GetByIdAsync(TourId);
			if (tour == null)
			{
				_logger.Information("Tour not found.");
				return new ApiResponse<int>(404, 0, "Tour not found.");
			}

			var review = tour.ReviewList.FirstOrDefault(r => r.Id == reviewId);
			if (review == null)
			{
				_logger.Information("Review not found.");
				return new ApiResponse<int>(404, 0, "Review not found.");
			}

			review.DeletedAt = DateTime.UtcNow;
			await _TourRepository.UpdateAsync(tour);

			_logger.Information($"End: ReviewTourService - DeleteReviewAsync : {reviewId} - Successfully deleted the review.");
			return new ApiResponse<int>(200, 1, "Review deleted successfully.");
		}
		catch (Exception ex)
		{
			_logger.Error($"Error in ReviewTourService - DeleteReviewAsync: {ex.Message}", ex);
			return new ApiResponse<int>(500, 0, $"An error occurred while deleting the review: {ex.Message}");
		}
	}
}
