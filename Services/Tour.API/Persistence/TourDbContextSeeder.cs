using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Shared.DTOs;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Tour.API.Entities;
using Tour.API.Persistence;
using ILogger = Serilog.ILogger;
namespace Room.API.Persistence
{
    public class TourDbContextSeeder
    {
        private readonly TourDbContext _context;
        private readonly ILogger _logger;

        public TourDbContextSeeder(TourDbContext context, ILogger logger)
        {
            this._context = context;
            this._logger = logger;
        }
<<<<<<< HEAD

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task InitialiseAsync()
        {
            try
            {
                _logger.Information("Initializing Db");
                if (_context.Database.IsMySql())
                {
                    await _context.Database.MigrateAsync();
                    _logger.Information("Initialed Db");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to Initialise database :{ex.Message}");
            }
        }
<<<<<<< HEAD

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public async Task SeedAsync()
        {
            try
            {
                _logger.Information("Seeding database...");

                // Seed DestinationEntity
                if (!_context.Destinations.Any())
                {
                    _context.Destinations.AddRange(new List<DestinationEntity>
                {
                    new DestinationEntity
                    {
                        Name = "Hà Nội",
                        Description = "Thủ đô ngàn năm văn hiến",
                        Url = "https://example.com/ha-noi"
                    },
                    new DestinationEntity
                    {
                        Name = "Hạ Long",
                        Description = "Vịnh Hạ Long - Kỳ quan thiên nhiên thế giới",
                        Url = "https://example.com/ha-long"
                    },
                    new DestinationEntity
                    {
                        Name = "Đà Nẵng",
                        Description = "Thành phố đáng sống",
                        Url = "https://example.com/da-nang"
                    },
                    new DestinationEntity
                    {
                        Name = "Phú Quốc",
                        Description = "Đảo ngọc Phú Quốc",
                        Url = "https://example.com/phu-quoc"
                    }
                });
                    await _context.SaveChangesAsync();
                    _logger.Information("Seeded DestinationEntity.");
                }

                // Seed TourEntity
                if (!_context.Tours.Any())
                {
                    var haNoi = _context.Destinations.First(d => d.Name == "Hà Nội");
                    var haLong = _context.Destinations.First(d => d.Name == "Hạ Long");
                    var daNang = _context.Destinations.First(d => d.Name == "Đà Nẵng");
                    var phuQuoc = _context.Destinations.First(d => d.Name == "Phú Quốc");

                    _context.Tours.AddRange(new List<TourEntity>
                {
                    new TourEntity
                    {
                        Name = "Tour Hà Nội",
                        MaxGuests = 15,
                        IsWifi = true,
                        Detail = "Khám phá thủ đô Hà Nội với nhiều di sản văn hóa và ẩm thực phong phú.",
                        Expect = "Thăm lăng Bác, Văn Miếu Quốc Tử Giám.",
                        Price = 300.00m,
                        DateFrom = DateTime.UtcNow.AddDays(7),
<<<<<<< HEAD
                        DateTo = DateTime.UtcNow.AddDays(30),
                        Rate = 4.9f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
=======
                        DateTo = DateTime.UtcNow.AddDays(10),
                        Rate = 4.9f,
                        Video = "https://example.com/ha-noi-tour-video",
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                        SalePercent = 5,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Di chuyển trong thành phố" },
                        ActivityList = new[] { "Tham quan", "Chụp ảnh" },
<<<<<<< HEAD
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
                        DayList = new[] {"Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7", "Day 8" },
                        ReviewList = new List <Review>
						{
                            new Review { Id = "1",  Content = "Chuyến đi tuyệt vời!", Rating = 5, CreatedAt = DateTime.UtcNow }
=======
                        ImageList = new[] { "https://example.com/ha-noi1.jpg", "https://example.com/ha-noi2.jpg" },
                        DayList = new[] { DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2) },
                        ReviewList = new Review[]
                        {
                            new Review { Content = "Chuyến đi tuyệt vời!", Rating = 5, CreatedAt = DateTime.UtcNow }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                        },
                        DestinationId = haNoi.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new TourEntity
                    {
                        Name = "Tour Hạ Long",
                        MaxGuests = 20,
                        IsWifi = true,
                        Detail = "Trải nghiệm vẻ đẹp hùng vĩ của vịnh Hạ Long.",
                        Expect = "Du thuyền trên vịnh, tham quan hang động.",
                        Price = 500.00m,
                        DateFrom = DateTime.UtcNow.AddDays(10),
<<<<<<< HEAD
                        DateTo = DateTime.UtcNow.AddDays(30),
                        Rate = 4.8f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 10,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Du thuyền" },
                        ActivityList = new[] { "Tham quan", "Leo núi" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
                        DayList = new[] { "Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7", "Day 8" },
                        ReviewList = new List <Review>
                        {
                            new Review { Id="1", Content = "Một kỳ nghỉ đáng nhớ!", Rating = 4.7f, CreatedAt = DateTime.UtcNow }
=======
                        DateTo = DateTime.UtcNow.AddDays(15),
                        Rate = 4.8f,
                        Video = "https://example.com/ha-long-tour-video",
                        SalePercent = 10,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Du thuyền" },
                        ActivityList = new[] { "Tham quan", "Leo núi" },
                        ImageList = new[] { "https://example.com/ha-long1.jpg", "https://example.com/ha-long2.jpg" },
                        DayList = new[] { DateTime.UtcNow.AddDays(11), DateTime.UtcNow.AddDays(12) },
                        ReviewList = new Review[]
                        {
                            new Review { Content = "Một kỳ nghỉ đáng nhớ!", Rating = 4.7f, CreatedAt = DateTime.UtcNow }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                        },
                        DestinationId = haLong.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new TourEntity
                    {
                        Name = "Tour Đà Nẵng",
                        MaxGuests = 25,
                        IsWifi = true,
                        Detail = "Tham quan thành phố Đà Nẵng hiện đại, thưởng thức ẩm thực địa phương.",
                        Expect = "Ngắm cầu Rồng, thăm Bà Nà Hills.",
                        Price = 400.00m,
                        DateFrom = DateTime.UtcNow.AddDays(15),
<<<<<<< HEAD
                        DateTo = DateTime.UtcNow.AddDays(30),
                        Rate = 4.85f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 12,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Xe đưa đón" },
                        ActivityList = new[] { "Tham quan", "Tắm biển" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
						DayList = new[] {"Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7", "Day 8" },
                        ReviewList = new List <Review>
						{
                            new Review { Id="1", Content = "Thành phố đẹp và dịch vụ rất tốt!", Rating = 4.8f, CreatedAt = DateTime.UtcNow }
=======
                        DateTo = DateTime.UtcNow.AddDays(20),
                        Rate = 4.85f,
                        Video = "https://example.com/da-nang-tour-video",
                        SalePercent = 12,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Xe đưa đón" },
                        ActivityList = new[] { "Tham quan", "Tắm biển" },
                        ImageList = new[] { "https://example.com/da-nang1.jpg", "https://example.com/da-nang2.jpg" },
                        DayList = new[] { DateTime.UtcNow.AddDays(16), DateTime.UtcNow.AddDays(17) },
                        ReviewList = new Review[]
                        {
                            new Review { Content = "Thành phố đẹp và dịch vụ rất tốt!", Rating = 4.8f, CreatedAt = DateTime.UtcNow }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                        },
                        DestinationId = daNang.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new TourEntity
                    {
                        Name = "Tour Phú Quốc",
                        MaxGuests = 30,
                        IsWifi = true,
                        Detail = "Khám phá thiên đường biển đảo Phú Quốc.",
                        Expect = "Tắm biển, ngắm hoàng hôn trên đảo.",
                        Price = 600.00m,
                        DateFrom = DateTime.UtcNow.AddDays(20),
<<<<<<< HEAD
                        DateTo = DateTime.UtcNow.AddDays(30),
                        Rate = 4.9f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 8,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Thuyền đưa đón" },
                        ActivityList = new[] { "Tắm biển", "Lặn ngắm san hô" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
						DayList = new[] {"Day 1", "Day 2", "Day 3", "Day 4", "Day 5", "Day 6", "Day 7", "Day 8" },
                        ReviewList = new List <Review>
						{
                            new Review { Id = "1",  Content = "Đảo ngọc đẹp như tranh vẽ!", Rating = 5, CreatedAt = DateTime.UtcNow }
=======
                        DateTo = DateTime.UtcNow.AddDays(25),
                        Rate = 4.9f,
                        Video = "https://example.com/phu-quoc-tour-video",
                        SalePercent = 8,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Thuyền đưa đón" },
                        ActivityList = new[] { "Tắm biển", "Lặn ngắm san hô" },
                        ImageList = new[] { "https://example.com/phu-quoc1.jpg", "https://example.com/phu-quoc2.jpg" },
                        DayList = new[] { DateTime.UtcNow.AddDays(21), DateTime.UtcNow.AddDays(22) },
                        ReviewList = new Review[]
                        {
                            new Review { Content = "Đảo ngọc đẹp như tranh vẽ!", Rating = 5, CreatedAt = DateTime.UtcNow }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                        },
                        DestinationId = phuQuoc.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                });
                    await _context.SaveChangesAsync();
                    _logger.Information("Seeded TourEntity.");
                }

                // Seed Schedule
                if (!_context.Schedules.Any())
                {
                    var haNoiTour = _context.Tours.First(t => t.Name == "Tour Hà Nội");
                    var haLongTour = _context.Tours.First(t => t.Name == "Tour Hạ Long");
                    var daNangTour = _context.Tours.First(t => t.Name == "Tour Đà Nẵng");
                    var phuQuocTour = _context.Tours.First(t => t.Name == "Tour Phú Quốc");
<<<<<<< HEAD
                    SeedScheduleForTour(haNoiTour);
                    SeedScheduleForTour(haLongTour);
                    SeedScheduleForTour(daNangTour);
                    SeedScheduleForTour(phuQuocTour);
=======

                    _context.Schedules.AddRange(new List<Schedule>
                {
                    new Schedule
                    {
                        DateStart = DateTime.UtcNow.AddDays(7),
                        DateEnd = DateTime.UtcNow.AddDays(10),
                        AvailableSeats = 15,
                        TourId = haNoiTour.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Schedule
                    {
                        DateStart = DateTime.UtcNow.AddDays(10),
                        DateEnd = DateTime.UtcNow.AddDays(15),
                        AvailableSeats = 20,
                        TourId = haLongTour.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Schedule
                    {
                        DateStart = DateTime.UtcNow.AddDays(15),
                        DateEnd = DateTime.UtcNow.AddDays(20),
                        AvailableSeats = 25,
                        TourId = daNangTour.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Schedule
                    {
                        DateStart = DateTime.UtcNow.AddDays(20),
                        DateEnd = DateTime.UtcNow.AddDays(25),
                        AvailableSeats = 30,
                        TourId = phuQuocTour.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                });
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
                    await _context.SaveChangesAsync();
                    _logger.Information("Seeded Schedule.");
                }

                _logger.Information("Database seeding completed.");
<<<<<<< HEAD

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while seeding the database: {ex.Message}");
            }
        }
<<<<<<< HEAD

        private void SeedScheduleForTour(TourEntity tour)
        {
            _logger.Information($"Seeding Schedule For {tour.Name}");
            var dateFrom = tour.DateFrom;
            var dateTo = tour.DateTo;
            while (dateFrom <= dateTo)
            {
                var dateStart = dateFrom;
                var dateEnd = dateFrom.AddDays(tour.DayList?.Count() ?? 0);

                if (dateEnd >= dateTo)
                {
                    break;
                }
                _context.Schedules.Add(new Schedule()
                {
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    AvailableSeats = tour.MaxGuests,
                    TourId = tour.Id,
                });
                dateFrom = dateFrom.AddDays(2);
            }
            _logger.Information($"Seeded Schedule For {tour.Name}");
        }
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
}
