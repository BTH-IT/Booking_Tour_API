using AutoMapper;
using Shared.DTOs;
using Shared.Helper;
using Tour.API.Repositories.Interfaces;
using Tour.API.Repositories.Services.Interfaces;
using ReviewEntity = Tour.API.Entities.Review;
using ILogger = Serilog.ILogger;


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

	public async Task<ApiResponse<Review>> CreateReviewAsync(Review reviewRequest)
	{
		_logger.Information("Begin: ReviewTourService  - CreateReviewAsync");

		var tour = await _TourRepository.GetTourByIdAsync(reviewRequest.TourId);
		if (tour == null)
		{
			return new ApiResponse<Review>(404, null, "Tour not found");
		}

		var reviewEntity = _mapper.Map<ReviewEntity>(reviewRequest);
		reviewEntity.CreatedAt = DateTime.UtcNow;

		if (tour.ReviewList == null)
		{
			tour.ReviewList = new List<ReviewEntity>();
		}

		tour.ReviewList.Add(reviewEntity);

		await _TourRepository.UpdateAsync(tour);

		var responseData = _mapper.Map<Review>(reviewEntity);
		_logger.Information("End: ReviewTourService  - CreateReviewAsync");
		return new ApiResponse<Review>(200, responseData, "Review created successfully");
	}

	public async Task<ApiResponse<Review>> UpdateReviewAsync(Review reviewRequest)
	{
		_logger.Information("Begin: ReviewTourService  - UpdateReviewAsync");

		var tour = await _TourRepository.GetTourByIdAsync(reviewRequest.TourId);
		if (tour == null)
		{
			return new ApiResponse<Review>(404, null, "Tour not found");
		}

		var review = tour.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
		if (review == null)
		{
			return new ApiResponse<Review>(404, null, "Review not found");
		}

		review.Content = reviewRequest.Content;
		review.Rating = reviewRequest.Rating;
		review.UpdatedAt = DateTime.UtcNow;

		await _TourRepository.UpdateAsync(tour);

		var responseData = _mapper.Map<Review>(review);
		_logger.Information("End: ReviewTourService  - UpdateReviewAsync");
		return new ApiResponse<Review>(200, responseData, "Review updated successfully");
	}

	public async Task<ApiResponse<int>> DeleteReviewAsync(int TourId, string reviewId)
	{
		_logger.Information($"Begin: ReviewTourService  - DeleteReviewAsync : {reviewId}");

		var tour = await _TourRepository.GetByIdAsync(TourId);
		if (tour == null)
		{
			return new ApiResponse<int>(404, 0, "Tour not found.");
		}

		var review = tour.ReviewList.FirstOrDefault(r => r.Id == reviewId);
		if (review == null)
		{
			return new ApiResponse<int>(404, 0, "Review not found.");
		}

		review.DeletedAt = DateTime.UtcNow;
		await _TourRepository.UpdateAsync(tour);

		_logger.Information($"End: ReviewTourService  - DeleteReviewAsync : {reviewId} - Successfully deleted the review.");
		return new ApiResponse<int>(200, 1, "Review deleted successfully.");
	}
}