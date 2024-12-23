﻿using AutoMapper;
using EventBus.IntergrationEvents.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Room.API.Entities;
using Room.API.Repositories.Interfaces;
using Room.API.Services.Interfaces;
using Shared.DTOs;
using Shared.Helper;
using ILogger = Serilog.ILogger;
using System.Text.Json;

namespace Room.API.Services
{
	public class ReviewHotelService : IReviewHotelService
	{
		private readonly IHotelRepository _hotelRepository;
		private readonly IMapper _mapper;
		private readonly ILogger _logger;
		private readonly IPublishEndpoint _publishEndpoint;
		private readonly IDistributedCache _cache;

		public ReviewHotelService(IHotelRepository hotelRepository, ILogger logger, IMapper mapper, IPublishEndpoint publishEndpoint, IDistributedCache cache)
		{
			_hotelRepository = hotelRepository;
			_logger = logger;
			_mapper = mapper;
			_publishEndpoint = publishEndpoint;
			_cache = cache;
		}

		public async Task<ApiResponse<ReviewHotelDTO>> CreateReviewAsync(ReviewHotelDTO reviewRequest)
		{
			_logger.Information("Begin: ReviewHotelService - CreateReviewAsync");

			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(reviewRequest.HotelId);
				if (hotel == null)
				{
					_logger.Warning("Hotel not found");
					return new ApiResponse<ReviewHotelDTO>(404, null, "Hotel not found");
				}

				var reviewEntity = _mapper.Map<ReviewHotel>(reviewRequest);
				reviewEntity.CreatedAt = DateTime.UtcNow;

				if (hotel.ReviewList == null)
				{
					hotel.ReviewList = new List<ReviewHotel>();
				}

				hotel.ReviewList.Add(reviewEntity);
				var result = await _hotelRepository.UpdateHotelAsync(hotel);

				if (result <= 0)
				{
					_logger.Warning("Failed to create review hotel");
					return new ApiResponse<ReviewHotelDTO>(400, null, "Error occurred while creating review hotel");
				}

				var responseData = _mapper.Map<ReviewHotelDTO>(reviewEntity);
				// publish event
				await _publishEndpoint.Publish(new ReviewHotelEvent()
				{
					Id = Guid.NewGuid(),
					ObjectId = reviewEntity.HotelId,
					Data = responseData,
					Type = "CREATE",
					CreationDate = DateTime.Now,
				});

				// Invalidate cache
				await _cache.RemoveAsync($"Hotel_All");
				await _cache.RemoveAsync($"Hotel_{reviewRequest.HotelId}");

				_logger.Information("End: ReviewHotelService - CreateReviewAsync");
				return new ApiResponse<ReviewHotelDTO>(200, responseData, "Review created successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error occurred while creating review: {ex.Message}");
				return new ApiResponse<ReviewHotelDTO>(500, null, $"An unexpected error occurred while created the review: {ex.Message}");
			}
		}

		public async Task<ApiResponse<ReviewHotelDTO>> UpdateReviewAsync(ReviewHotelDTO reviewRequest)
		{
			_logger.Information("Begin: ReviewHotelService - UpdateReviewAsync");

			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(reviewRequest.HotelId);
				if (hotel == null)
				{
					_logger.Warning("Hotel not found");
					return new ApiResponse<ReviewHotelDTO>(404, null, "Hotel not found");
				}

				var review = hotel.ReviewList.FirstOrDefault(r => r.Id == reviewRequest.Id);
				if (review == null)
				{
					_logger.Warning("Review not found");
					return new ApiResponse<ReviewHotelDTO>(404, null, "Review not found");
				}

				review.Content = reviewRequest.Content;
				review.Rating = reviewRequest.Rating;
				review.UpdatedAt = DateTime.UtcNow;

				var result = await _hotelRepository.UpdateHotelAsync(hotel);
				if (result <= 0)
				{
					_logger.Warning("Failed to Update review hotel");
					return new ApiResponse<ReviewHotelDTO>(400, null, "Error occurred while updated review hotel");
				}

				var responseData = _mapper.Map<ReviewHotelDTO>(review);
				// publish event
				await _publishEndpoint.Publish(new ReviewHotelEvent()
				{
					Id = Guid.NewGuid(),
					ObjectId = responseData.HotelId,
					Data = responseData,
					Type = "UPDATE",
					CreationDate = DateTime.Now,
				});

				// Invalidate cache
				await _cache.RemoveAsync($"Hotel_All");
				await _cache.RemoveAsync($"Hotel_{reviewRequest.HotelId}");

				_logger.Information("End: ReviewHotelService - UpdateReviewAsync");
				return new ApiResponse<ReviewHotelDTO>(200, responseData, "Review updated successfully");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error occurred while updating review: {ex.Message}");
				return new ApiResponse<ReviewHotelDTO>(500, null, $"An unexpected error occurred while updating the review: {ex.Message}");
			}
		}

		public async Task<ApiResponse<int>> DeleteReviewAsync(int hotelId, string reviewId)
		{
			_logger.Information($"Begin: ReviewHotelService - DeleteReviewAsync: {reviewId}");

			try
			{
				var hotel = await _hotelRepository.GetHotelByIdAsync(hotelId);
				if (hotel == null)
				{
					_logger.Warning("Hotel not found");
					return new ApiResponse<int>(404, 0, "Hotel not found.");
				}

				var review = hotel.ReviewList.FirstOrDefault(r => r.Id == reviewId);
				if (review == null)
				{
					_logger.Warning("Review not found");
					return new ApiResponse<int>(404, 0, "Review not found.");
				}

				review.DeletedAt = DateTime.UtcNow;
				var result = await _hotelRepository.UpdateHotelAsync(hotel);
				if (result <= 0)
				{
					_logger.Warning("Failed to deleted review hotel");
					return new ApiResponse<int>(400, 0, "Error occurred while deleted review hotel");
				}
				// publish event
				await _publishEndpoint.Publish(new ReviewHotelEvent()
				{
					Id = Guid.NewGuid(),
					ObjectId = review.HotelId,
					Data = _mapper.Map<ReviewHotelDTO>(review),
					Type = "DELETE",
					CreationDate = DateTime.Now,
				});

				// Invalidate cache
				await _cache.RemoveAsync($"Hotel_All");
				await _cache.RemoveAsync($"Hotel_{hotelId}");

				_logger.Information($"End: ReviewHotelService - DeleteReviewAsync: {reviewId} - Successfully deleted the review.");
				return new ApiResponse<int>(200, 1, "Review deleted successfully.");
			}
			catch (Exception ex)
			{
				_logger.Error($"Unexpected error occurred while deleting review: {ex.Message}");
				return new ApiResponse<int>(500, 0, $"An unexpected error occurred while deleting the review: {ex.Message}");
			}
		}
	}
}