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
		_logger.Information("Begin: ReviewRoomService - CreateReviewAsync");

		try
		{
			var room = await _roomRepository.GetRoomByIdAsync(reviewRequest.RoomId);
			if (room == null)
			{
				_logger.Warning("Room not found");
				return new ApiResponse<ReviewRoomDTO>(404, null, "Room not found");
			}

			var reviewEntity = _mapper.Map<ReviewRoom>(reviewRequest);
			reviewEntity.CreatedAt = DateTime.UtcNow;

			if (room.ReviewList == null)
			{
				room.ReviewList = new List<ReviewRoom>();
			}
			room.ReviewList.Add(reviewEntity);

			var result = await _roomRepository.UpdateRoomAsync(room);
			if (result <= 0)
			{
				_logger.Warning("Failed to create review for room");
				return new ApiResponse<ReviewRoomDTO>(400, null, "Error occurred while creating review for room");
			}

			var responseData = _mapper.Map<ReviewRoomDTO>(reviewEntity);
			_logger.Information("End: ReviewRoomService - CreateReviewAsync");
			return new ApiResponse<ReviewRoomDTO>(200, responseData, "Review created successfully");
		}
		catch (Exception ex)
		{
			_logger.Error($"Unexpected error occurred while creating review: {ex.Message}");
			return new ApiResponse<ReviewRoomDTO>(500, null, $"An unexpected error occurred while creating the review: {ex.Message}");
		}
	}

	public async Task<ApiResponse<ReviewRoomDTO>> UpdateReviewAsync(ReviewRoomDTO reviewRequest)
	{
		_logger.Information("Begin: ReviewRoomService - UpdateReviewAsync");

		try
		{
			var room = await _roomRepository.GetRoomByIdAsync(reviewRequest.RoomId);
			if (room == null)
			{
				_logger.Warning("Room not found");
				return new ApiResponse<ReviewRoomDTO>(404, null, "Room not found");
			}

			var review = room.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
			if (review == null)
			{
				_logger.Warning("Review not found");
				return new ApiResponse<ReviewRoomDTO>(404, null, "Review not found");
			}

			review.Content = reviewRequest.Content;
			review.Rating = reviewRequest.Rating;
			review.UpdatedAt = DateTime.UtcNow;

			var result = await _roomRepository.UpdateRoomAsync(room);
			if (result <= 0)
			{
				_logger.Warning("Failed to update review for room");
				return new ApiResponse<ReviewRoomDTO>(400, null, "Error occurred while updating review for room");
			}

			var responseData = _mapper.Map<ReviewRoomDTO>(review);
			_logger.Information("End: ReviewRoomService - UpdateReviewAsync");
			return new ApiResponse<ReviewRoomDTO>(200, responseData, "Review updated successfully");
		}
		catch (Exception ex)
		{
			_logger.Error($"Unexpected error occurred while updating review: {ex.Message}");
			return new ApiResponse<ReviewRoomDTO>(500, null, $"An unexpected error occurred while updating the review: {ex.Message}");
		}
	}

	public async Task<ApiResponse<int>> DeleteReviewAsync(int roomId, string reviewId)
	{
		_logger.Information($"Begin: ReviewRoomService - DeleteReviewAsync: {reviewId}");

		try
		{
			var room = await _roomRepository.GetRoomByIdAsync(roomId);
			if (room == null)
			{
				_logger.Warning("Room not found");
				return new ApiResponse<int>(404, 0, "Room not found.");
			}

			var review = room.ReviewList.FirstOrDefault(r => r.Id == reviewId);
			if (review == null)
			{
				_logger.Warning("Review not found");
				return new ApiResponse<int>(404, 0, "Review not found.");
			}

			review.DeletedAt = DateTime.UtcNow;
			var result = await _roomRepository.UpdateRoomAsync(room);
			if (result <= 0)
			{
				_logger.Warning("Failed to delete review for room");
				return new ApiResponse<int>(400, 0, "Error occurred while deleting review for room");
			}

			_logger.Information($"End: ReviewRoomService - DeleteReviewAsync: {reviewId} - Successfully deleted the review.");
			return new ApiResponse<int>(200, 1, "Review deleted successfully.");
		}
		catch (Exception ex)
		{
			_logger.Error($"Unexpected error occurred while deleting review: {ex.Message}");
			return new ApiResponse<int>(500, 0, $"An unexpected error occurred while deleting the review: {ex.Message}");
		}
	}
}
