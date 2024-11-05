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
            // Seed 100 BookingRooms
            if (!_context.BookingRooms.Any())
            {
                var random = new Random();
                for (int i = 1; i <= 100; i++)
                {
                    var bookingRoom = new BookingRoom
                    {
                        UserId = random.Next(1, 10),
                        CheckIn = DateTime.Now.AddDays(-random.Next(0, 60)),
                        CheckOut = DateTime.Now.AddDays(-random.Next(0, 60) + random.Next(1, 5)),
                        NumberOfPeople = random.Next(1, 5),
                        PriceTotal = random.NextDouble() * 500 + 100,
                        CreatedAt = DateTime.Now,
                        DetailBookingRooms = new List<DetailBookingRoom>
                {
                    new DetailBookingRoom
                    {
                        RoomId = random.Next(1, 100),
                        Price = random.NextDouble() * 200 + 50,
                        Adults = random.Next(1, 3),
                        Children = random.Next(0, 2),
                        CreatedAt = DateTime.Now
                    }
                }
                    };

                    await _context.BookingRooms.AddAsync(bookingRoom);
                }
            }

            // Seed 100 BookingTours
            if (!_context.BookingTours.Any())
            {
                var random = new Random();
                for (int i = 1; i <= 100; i++)
                {
                    var seats = random.Next(1, 5);
                    var travellers = new List<Traveller>();
                    for (int j = 1; j <= seats; j++)
                    {
                        travellers.Add(new Traveller
                        {
                            Gender = j % 2 == 0 ? "Nam" : "Nữ",
                            Fullname = $"Traveller {i}-{j}",
                            Age = 20,
                            Phone = "012345678" + random.Next(1, 10)
                        });
                    }

                    var bookingTour = new BookingTour
                    {
                        UserId = random.Next(1, 10),
                        ScheduleId = random.Next(1, 10),
                        Seats = seats,
                        IsTip = random.Next(0, 2) == 1,
                        IsEntranceTicket = random.Next(0, 2) == 1,
                        Status = "true",
                        PriceTotal = random.NextDouble() * 1000 + 300,
                        CreatedAt = DateTime.Now,
                        Travellers = JsonConvert.SerializeObject(travellers)
                    };

                    await _context.BookingTours.AddAsync(bookingTour);
                }
            }

            await _context.SaveChangesAsync();
            _logger.Information("Seeded initial data to Db");
        }
    }
}
