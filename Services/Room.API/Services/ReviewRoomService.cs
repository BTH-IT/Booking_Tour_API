using AutoMapper;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;

public class ReviewRoomService : IReviewRoomService
{
	private readonly IRoomRepository _roomRepository;
	private readonly IMapper _mapper;
	private readonly ILogger _logger;

	public ReviewRoomService(IRoomRepository roomRepository, ILogger logger, IMapper mapper)
	{
		_roomRepository = roomRepository;
		_logger = logger;
		_mapper = mapper;
	}

	public async Task<ApiResponse<ReviewRoomDTO>> CreateReviewAsync(ReviewRoomDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewRoomService  - CreateReviewAsync");

		var room = await _roomRepository.GetRoomByIdAsync(reviewRequest.RoomId);
		if (room == null)
		{
			return new ApiResponse<ReviewRoomDTO>(404, null, "Room not found");
		}

		var reviewEntity = _mapper.Map<ReviewRoom>(reviewRequest);
		reviewEntity.CreatedAt = DateTime.UtcNow;
		room.ReviewList.Add(reviewEntity);

		await _roomRepository.UpdateAsync(room);

		var responseData = _mapper.Map<ReviewRoomDTO>(reviewEntity);
		_logger.Information("End: ReviewRoomService  - CreateReviewAsync");
		return new ApiResponse<ReviewRoomDTO>(201, responseData, "Review created successfully");
	}

	public async Task<ApiResponse<ReviewRoomDTO>> UpdateReviewAsync(ReviewRoomDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewroomService  - UpdateReviewAsync");

		var room = await _roomRepository.GetRoomByIdAsync(reviewRequest.RoomId);
		if (room == null)
		{
			return new ApiResponse<ReviewRoomDTO>(404, null, "Room not found");
		}

		var review = room.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
		if (review == null)
		{
			return new ApiResponse<ReviewRoomDTO>(404, null, "Review not found");
		}

		review.Content = reviewRequest.Content;
		review.Rating = reviewRequest.Rating;
		review.UpdatedAt = DateTime.UtcNow;

		await _roomRepository.UpdateAsync(room);

		var responseData = _mapper.Map<ReviewRoomDTO>(review);
		_logger.Information("End: ReviewRoomService  - UpdateReviewAsync");
		return new ApiResponse<ReviewRoomDTO>(200, responseData, "Review updated successfully");
	}

	public async Task<ApiResponse<int>> DeleteReviewAsync(int roomId, string reviewId)
	{
		_logger.Information($"Begin: ReviewRoomService  - DeleteReviewAsync : {reviewId}");

		var room = await _roomRepository.GetByIdAsync(roomId);
		if (room == null)
		{
			return new ApiResponse<int>(404, 0, "Room not found.");
		}

		var review = room.ReviewList.FirstOrDefault(r => r.Id == reviewId);
		if (review == null)
		{
			return new ApiResponse<int>(404, 0, "Review not found.");
		}

		review.DeletedAt = DateTime.UtcNow;
		await _roomRepository.UpdateAsync(room);

		_logger.Information($"End: ReviewRoomService  - DeleteReviewAsync : {reviewId} - Successfully deleted the review.");
		return new ApiResponse<int>(200, 1, "Review deleted successfully.");
	}
}