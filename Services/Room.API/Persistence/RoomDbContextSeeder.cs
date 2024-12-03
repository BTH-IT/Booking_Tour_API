﻿using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
using Room.API.Entities; 

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
					_logger.Information("Đang seed dữ liệu khách sạn, phòng, hình ảnh, video, đánh giá...");

					var provinces = new[]
					{
						"Hà Nội", "TP.HCM", "Đà Nẵng", "Hải Phòng", "Cần Thơ", "Bình Dương", "Đồng Nai", "Bà Rịa - Vũng Tàu", "Khánh Hòa", "Quảng Ninh",
						"Thừa Thiên Huế", "Bình Thuận", "Lâm Đồng", "Bình Phước", "Quảng Nam", "Nghệ An", "Thanh Hóa", "Hà Tĩnh", "Quảng Bình", "Quảng Trị",
						"Bắc Ninh", "Bắc Giang", "Thái Nguyên", "Nam Định", "Hà Nam", "Ninh Bình", "Lào Cai", "Yên Bái", "Điện Biên", "Sơn La",
						"Lai Châu", "Hòa Bình", "Phú Thọ", "Tuyên Quang", "Cao Bằng", "Bắc Kạn", "Lạng Sơn", "Thái Bình", "Hà Giang", "Vĩnh Phúc",
						"Quảng Ngãi", "Bình Định", "Phú Yên", "Gia Lai", "Kon Tum", "Đắk Lắk", "Đắk Nông", "Sóc Trăng", "Trà Vinh", "Vĩnh Long",
						"Tiền Giang", "Bến Tre", "An Giang", "Kiên Giang", "Hậu Giang", "Bạc Liêu", "Cà Mau", "Ninh Thuận", "Tây Ninh", "Long An",
						"Đồng Tháp", "Bình Phước", "Hải Dương", "Nam Định", "Thái Bình", "Nghệ An"
					};

					var hotels = new List<Hotel>();
					int count = 1;

					foreach (var province in provinces)
					{
						for (int i = 1; i <= 5; i++)
						{
							var hotel = new Hotel
							{
								Name = $"Khách sạn {province} {i}",
								Location = $"{province}",
								LocationCode = count,
								Description = $"Mô tả cho Khách sạn {province} {i}",
								ContactInfo = $"+84-24-1234-567{i}",
								CreatedAt = DateTime.UtcNow,
								UpdatedAt = DateTime.UtcNow,
								ReviewList = new List<ReviewHotel>(),
<<<<<<< HEAD
=======
								HotelRulesList = new List<HotelRules>(),
								HotelAmenitiesList = new List<HotelAmenities>(),
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
								Rooms = new List<RoomEntity>()
							};

							for (int j = 1; j <= 5; j++)
							{
								var room = new RoomEntity
								{
									Name = $"Phòng {j} tại {hotel.Name}",
<<<<<<< HEAD
									ImagesList = new string[5],
									Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
									Detail = $"Chi tiết về phòng {j} tại {hotel.Name}",
									Price = 100 + (j * 20),
									IsAvailable = true,
									MaxGuests = 2,
									CreatedAt = DateTime.UtcNow,
									ReviewList = new List<ReviewRoom>()
=======
									Detail = $"Chi tiết về phòng {j} tại {hotel.Name}",
									Price = 100 + (j * 20), 
									IsAvailable = true,
									MaxGuests = 2,
									CreatedAt = DateTime.UtcNow,
									ReviewList = new List<ReviewRoom>(),
									RoomAmenitiesList = new List<RoomAmenities>(),
									ImagesList = new List<Image>(),
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
								};
								int Counter = 0;
								for (int k = 1; k <= 5; k++)
								{
<<<<<<< HEAD
									room.ImagesList[k - 1] = $"https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg";
=======
									room.ImagesList.Add(new Image
									{
										Id = $"{++Counter}",
										Url = $"https://realimages.com/{province}/hotel{i}/room{j}/image{k}.jpg"
									});
									
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
									room.ReviewList.Add(new ReviewRoom
									{
										Id = $"{Counter}",
										Content = $"Đánh giá tuyệt vời cho phòng {j} tại khách sạn {province} {i}.",
										Rating = 4.5f,
										CreatedAt = DateTime.UtcNow,
										RoomId = room.Id,
										UserId = j
									});
								}
<<<<<<< HEAD
								room.RoomAmenitiesList = new[] { "Wi-Fi miễn phí", "Điều hòa nhiệt độ", "TV màn hình phẳng" };
=======
								room.VideoObject = new Video
								{
									Id = "1",
									Url = "https://example.com/video1.mp4",
									Type = "mp4"
								};
								room.RoomAmenitiesList.Add(new RoomAmenities { Id = $"1", Title = "Wi-Fi miễn phí" });
								room.RoomAmenitiesList.Add(new RoomAmenities { Id = $"2", Title = "Điều hòa nhiệt độ" });
								room.RoomAmenitiesList.Add(new RoomAmenities { Id = $"3", Title = "TV màn hình phẳng" });
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

								hotel.Rooms.Add(room);
							}
							int Count = 0;
							for (int k = 1; k <= 5; k++)
							{
								hotel.ReviewList.Add(new ReviewHotel
								{
									Id = $"{++Count}",
									Content = $"Excellent hotel experience. Review {k} for Hotel {i}",
									Rating = 4 + (k % 2),
									CreatedAt = DateTime.UtcNow,
									HotelId = hotel.Id,
									UserId = k + 100
								});
							}

<<<<<<< HEAD
							hotel.HotelRulesList = new[] { "Không hút thuốc trong phòng.", "Giữ yên lặng từ 22h đến 6h sáng." };
							hotel.HotelAmenitiesList = new[] { "Bể bơi ngoài trời", "Phòng Gym", "Spa" };
=======
							hotel.HotelRulesList.Add(new HotelRules { Id = $"1", Title = "Không hút thuốc trong phòng." });
							hotel.HotelRulesList.Add(new HotelRules { Id = $"2", Title = "Giữ yên lặng từ 22h đến 6h sáng." });
							hotel.HotelAmenitiesList.Add(new HotelAmenities { Id = $"1", Title = "Bể bơi ngoài trời" });
							hotel.HotelAmenitiesList.Add(new HotelAmenities { Id = $"2", Title = "Phòng Gym" });
							hotel.HotelAmenitiesList.Add(new HotelAmenities { Id = $"3", Title = "Spa" });
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

							hotels.Add(hotel);
						}
						count++;
					}

<<<<<<< HEAD


=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
					await _context.Hotels.AddRangeAsync(hotels);
					await _context.SaveChangesAsync();

					_logger.Information("Seed dữ liệu hoàn thành.");
				}
				else
				{
					_logger.Information("Dữ liệu khách sạn đã tồn tại, bỏ qua seed.");
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Lỗi trong quá trình seed dữ liệu: {ex.Message}");
			}
		}
	}
}
