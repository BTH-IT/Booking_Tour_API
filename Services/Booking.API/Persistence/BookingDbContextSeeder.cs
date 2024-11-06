using Booking.API.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Booking.API.Persistence
{
	public class BookingDbContextSeeder
	{
		private readonly BookingDbContext _context;
		private readonly ILogger _logger;

		public BookingDbContextSeeder(BookingDbContext context, ILogger logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task InitialiseAsync()
		{
			try
			{
				_logger.Information("Initializing Db");
				if (_context.Database.IsMySql())
				{
					await _context.Database.MigrateAsync();
					_logger.Information("Initialized Db");

					// Seed data
					await SeedDataAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.Error($"Failed to Initialise database :{ex.Message}");
			}
		}

		private async Task SeedDataAsync()
		{
			// Seed BookingRooms and BookingTours (same as before)
			if (!_context.BookingRooms.Any())
			{
				var bookingRoom1 = new BookingRoom
				{
					UserId = 1,
					CheckIn = DateTime.Now,
					CheckOut = DateTime.Now.AddDays(3),
					NumberOfPeople = 2,
					PriceTotal = 150.00,
					CreatedAt = DateTime.Now,
					DetailBookingRooms = new List<DetailBookingRoom>
					{
						new DetailBookingRoom
						{
							RoomId = 1,
							Price = 75.00,
							Adults = 2,
							Children = 0,
							CreatedAt = DateTime.Now
						},
						new DetailBookingRoom
						{
							RoomId = 2,
							Price = 75.00,
							Adults = 0,
							Children = 2,
							CreatedAt = DateTime.Now
						}
					}
				};

				var bookingRoom2 = new BookingRoom
				{
					UserId = 2,
					CheckIn = DateTime.Now.AddDays(1),
					CheckOut = DateTime.Now.AddDays(4),
                    NumberOfPeople = 3,
					PriceTotal = 200.00,
					CreatedAt = DateTime.Now,
					DetailBookingRooms = new List<DetailBookingRoom>
					{
						new DetailBookingRoom
						{
							RoomId = 3,
							Price = 100.00,
							Adults = 2,
							Children = 1,
							CreatedAt = DateTime.Now
						}
					}
				};

				await _context.BookingRooms.AddRangeAsync(bookingRoom1, bookingRoom2);
			}

			if (!_context.BookingTours.Any())
			{
                var bookingTour1 = new BookingTour
				{
					UserId = 1,
					ScheduleId = 1,
					Seats = 4,
					Umbrella = true,
					IsCleaningFee = false,
					IsTip = true,
					IsEntranceTicket = true,
					Status = true,
					PriceTotal = 500.00,
					Coupon = 50.00,
					DateStart = DateTime.ParseExact("2024-10-27 04:22:09.812176", "yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture),	
					DateEnd = DateTime.ParseExact("2024-10-30 04:22:09.812238", "yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture),
					PaymentMethod = 1,
					CreatedAt = DateTime.Now,
					Travellers = JsonConvert.SerializeObject(new[]
					{
						new Traveller { Gender = true, Fullname = "Nguyễn Văn A", Age = 30, Phone = "0123456789" },
						new Traveller { Gender = false, Fullname = "Trần Thị B", Age = 28, Phone = "0987654321" }
					})
				};

				var bookingTour2 = new BookingTour
				{
					UserId = 2,
					ScheduleId = 2,
					Seats = 2,
					Umbrella = false,
					IsCleaningFee = true,
					IsTip = false,
					IsEntranceTicket = false,
					Status = true,
                    DateStart = DateTime.ParseExact("2024-10-30 04:22:09.812598", "yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture),
                    DateEnd = DateTime.ParseExact("2024-11-04 04:22:09.812599", "yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture),
                    PriceTotal = 300.00,
					Coupon = 20.00,
					PaymentMethod = 2,
					CreatedAt = DateTime.Now,
					Travellers = JsonConvert.SerializeObject(new[]
					{
						new Traveller { Gender = true, Fullname = "Lê Văn C", Age = 35, Phone = "0111222333" },
						new Traveller { Gender = false, Fullname = "Phạm Thị D", Age = 32, Phone = "0222333444" }
					})
				};

				await _context.BookingTours.AddRangeAsync(bookingTour1, bookingTour2);
			}

			await _context.SaveChangesAsync();
			_logger.Information("Seeded initial data to Db");
		}
	}
}
