using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger; // Use Serilog for logging
using Room.API.Entities; // Ensure this is the correct namespace for your entities
using System.Collections.Generic;
using Shared.Enums; // For List<T>

namespace Room.API.Persistence
{
	public class RoomDbContextSeeder
	{
		private readonly RoomDbContext _context;
		private readonly ILogger _logger;

		public RoomDbContextSeeder(RoomDbContext context, ILogger logger)
		{
			_context = context;
			_logger = logger;
		}

		// Method to initialize the database
		public async Task InitialiseAsync()
		{
			try
			{
				_logger.Information("Initializing Database");
				if (_context.Database.IsMySql())
				{
					await _context.Database.MigrateAsync(); // Apply migrations
					_logger.Information("Database Initialized");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Failed to Initialize database: {ex.Message}");
			}
		}
		public async Task RoomDbSeedAsync()
		{
			try
			{
				if (!_context.Hotels.Any())
				{
					_logger.Information("Seeding database with hotels, rooms, reviews, and hotel rules...");

					var hotels = new List<Hotel>();

					for (int i = 1; i <= 10; i++)
					{
						var hotel = new Hotel
						{
							Name = $"Hotel {i}",
							Location = $"Address {i}, City, Country",
							Description = $"Description for Hotel {i}",
							ContactInfo = $"+1-305-{i}23-4567",
							Rate = 4.5 + (i * 0.02), 
							CreatedAt = DateTime.UtcNow,
							UpdatedAt = DateTime.UtcNow,
							ReviewList = new List<ReviewHotel>(),
							HotelRulesList = new List<HotelRules>(),
							Rooms = new List<RoomEntity>()
						};

						for (int j = 1; j <= 10; j++)
						{
							var room = new RoomEntity
							{
								Name = $"Room {j} in Hotel {i}",
								Type = (RoomType) 2, 
								BedType = (BedType) 1, 
								Rate = 4.5,
								Detail = $"Detail for Room {j} in Hotel {i}",
								Price = 100 + (j * 10), 
								IsAvailable = true,
								MaxGuests = 2,
								Size = 40 + (j * 5), 
								CreatedAt = DateTime.UtcNow,
								UpdatedAt = DateTime.UtcNow,
								ReviewList = new List<ReviewRoom>(),
								RoomAmenitiesList = new List<RoomAmenities>(),
								HotelAmenitiesList = new List<HotelAmenities>()
							};

							room.ReviewList.Add(new ReviewRoom
							{
								Content = $"Amazing room with a great view. Review {j} for Room {j} in Hotel {i}",
								Rating = 5,
								CreatedAt = DateTime.UtcNow,
								UpdatedAt = DateTime.UtcNow,
								RoomId = room.Id,
								UserId = j 
							});

							room.RoomAmenitiesList.Add(new RoomAmenities { Title = "Free Wi-Fi" });
							room.RoomAmenitiesList.Add(new RoomAmenities { Title = "Air Conditioning" });
							room.RoomAmenitiesList.Add(new RoomAmenities { Title = "Flat Screen TV" });

							room.HotelAmenitiesList.Add(new HotelAmenities { Title = "Outdoor Pool" });
							room.HotelAmenitiesList.Add(new HotelAmenities { Title = "Gym" });
							room.HotelAmenitiesList.Add(new HotelAmenities { Title = "Spa" });

							hotel.Rooms.Add(room);
						}

						for (int k = 1; k <= 5; k++) 
						{
							hotel.ReviewList.Add(new ReviewHotel
							{
								Content = $"Excellent hotel experience. Review {k} for Hotel {i}",
								Rating = 4 + (k % 2), 
								CreatedAt = DateTime.UtcNow,
								UpdatedAt = DateTime.UtcNow,
								HotelId = hotel.Id,
								UserId = k + 100 
							});
						}

						hotel.HotelRulesList.Add(new HotelRules { Title = "No pets allowed." });
						hotel.HotelRulesList.Add(new HotelRules { Title = "Quiet hours from 10 PM to 6 AM." });

						hotels.Add(hotel);
					}

					await _context.Hotels.AddRangeAsync(hotels);
					await _context.SaveChangesAsync();

					_logger.Information("Seeding complete.");
				}
				else
				{
					_logger.Information("Hotels already exist in the database. Skipping seeding.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"An error occurred while seeding the database: {ex.Message}");
			}
		}
	}
}
