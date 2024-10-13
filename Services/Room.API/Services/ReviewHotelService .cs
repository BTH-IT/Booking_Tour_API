using AutoMapper;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

public class ReviewHotelService : IReviewHotelService
{
	private readonly IHotelRepository _hotelRepository;  
	private readonly IMapper _mapper;
	private readonly ILogger _logger;

	public ReviewHotelService(IHotelRepository hotelRepository, ILogger logger, IMapper mapper)
	{
		_hotelRepository = hotelRepository; 
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<ApiResponse<ReviewHotelDTO>> CreateReviewAsync(ReviewHotelDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewHotelService  - CreateReviewAsync");

		var hotel = await _hotelRepository.GetHotelByIdAsync(reviewRequest.HotelId);
		if (hotel == null)
		{
			return new ApiResponse<ReviewHotelDTO>(404, null, "Hotel not found");
		}

		var reviewEntity = _mapper.Map<ReviewHotel>(reviewRequest);
		reviewEntity.CreatedAt = DateTime.UtcNow;

		if (hotel.ReviewList == null)
		{
			hotel.ReviewList = new List<ReviewHotel>();
		}

		hotel.ReviewList.Add(reviewEntity); 

		await _hotelRepository.UpdateAsync(hotel); 

		var responseData = _mapper.Map<ReviewHotelDTO>(reviewEntity); 
		_logger.Information("End: ReviewHotelService  - CreateReviewAsync");
		return new ApiResponse<ReviewHotelDTO>(200, responseData, "Review created successfully");
	}

	public async Task<ApiResponse<ReviewHotelDTO>> UpdateReviewAsync(ReviewHotelDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewHotelService  - UpdateReviewAsync");

		var hotel = await _hotelRepository.GetHotelByIdAsync(reviewRequest.HotelId);
		if (hotel == null)
		{
			return new ApiResponse<ReviewHotelDTO>(404, null, "Hotel not found");
		}

		var review = hotel.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
		if (review == null)
		{
			return new ApiResponse<ReviewHotelDTO>(404, null, "Review not found");
		}

		review.Content = reviewRequest.Content;
		review.Rating = reviewRequest.Rating;
		review.UpdatedAt = DateTime.UtcNow;

		await _hotelRepository.UpdateAsync(hotel); 

		var responseData = _mapper.Map<ReviewHotelDTO>(review);
		_logger.Information("End: ReviewHotelService  - UpdateReviewAsync");
		return new ApiResponse<ReviewHotelDTO>(200, responseData, "Review updated successfully");
	}

	public async Task<ApiResponse<int>> DeleteReviewAsync(int hotelId, string reviewId)
	{
		_logger.Information($"Begin: ReviewHotelService  - DeleteReviewAsync : {reviewId}");

		var hotel = await _hotelRepository.GetByIdAsync(hotelId);
		if (hotel == null)
		{
			return new ApiResponse<int>(404, 0, "Hotel not found.");
		}

		var review = hotel.ReviewList.FirstOrDefault(r => r.Id == reviewId);
		if (review == null)
		{
			return new ApiResponse<int>(404, 0, "Review not found.");
		}

		review.DeletedAt = DateTime.UtcNow; 
		await _hotelRepository.UpdateAsync(hotel); 

		_logger.Information($"End: ReviewHotelService  - DeleteReviewAsync : {reviewId} - Successfully deleted the review.");
		return new ApiResponse<int>(200, 1, "Review deleted successfully.");
	}
}