using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
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
                        DateTo = DateTime.UtcNow.AddDays(10),
                        Rate = 4.9f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
                        SalePercent = 5,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Di chuyển trong thành phố" },
                        ActivityList = new[] { "Tham quan", "Chụp ảnh" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
                        DayList = new[] {"Day 1", "Day 2" },
                        ReviewList = new List <Review>
						{
                            new Review { Id = "1",  Content = "Chuyến đi tuyệt vời!", Rating = 5, CreatedAt = DateTime.UtcNow }
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
                        DateTo = DateTime.UtcNow.AddDays(15),
                        Rate = 4.8f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 10,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Du thuyền" },
                        ActivityList = new[] { "Tham quan", "Leo núi" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
                        DayList = new[] { "Day 1", "Day 2" },
                        ReviewList = new List <Review>
                        {
                            new Review { Id="1", Content = "Một kỳ nghỉ đáng nhớ!", Rating = 4.7f, CreatedAt = DateTime.UtcNow }
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
                        DateTo = DateTime.UtcNow.AddDays(20),
                        Rate = 4.85f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 12,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Xe đưa đón" },
                        ActivityList = new[] { "Tham quan", "Tắm biển" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
						DayList = new[] { "Day 1", "Day 2" },
                        ReviewList = new List <Review>
						{
                            new Review { Id="1", Content = "Thành phố đẹp và dịch vụ rất tốt!", Rating = 4.8f, CreatedAt = DateTime.UtcNow }
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
                        DateTo = DateTime.UtcNow.AddDays(25),
                        Rate = 4.9f,
						Video = "https://booking-cloud-storage.s3.amazonaws.com/tour.mp4",
						SalePercent = 8,
                        PriceExcludeList = new[] { "Bữa ăn", "Vé máy bay" },
                        PriceIncludeList = new[] { "Khách sạn", "Thuyền đưa đón" },
                        ActivityList = new[] { "Tắm biển", "Lặn ngắm san hô" },
                        ImageList = new[] { "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg", "https://booking-cloud-storage.s3.amazonaws.com/jack-ward-rknrvCrfS1k-unsplash-scaled.jpg" },
						DayList = new[] { "Day 1", "Day 2" },
                        ReviewList = new List <Review>
						{
                            new Review { Id = "1",  Content = "Đảo ngọc đẹp như tranh vẽ!", Rating = 5, CreatedAt = DateTime.UtcNow }
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
                    SeedScheduleForTour(haNoiTour);
                    SeedScheduleForTour(haLongTour);
                    SeedScheduleForTour(daNangTour);
                    SeedScheduleForTour(phuQuocTour);
                    await _context.SaveChangesAsync();
                    _logger.Information("Seeded Schedule.");
                }

                _logger.Information("Database seeding completed.");

                // Seed TourRoom
                if (!_context.TourRooms.Any())
                {
                    var haNoiTour = _context.Tours.First(t => t.Name == "Tour Hà Nội");
                    var haLongTour = _context.Tours.First(t => t.Name == "Tour Hạ Long");
                    var daNangTour = _context.Tours.First(t => t.Name == "Tour Đà Nẵng");
                    var phuQuocTour = _context.Tours.First(t => t.Name == "Tour Phú Quốc");


                    // Thêm TourRoom cho từng tour
                    _context.TourRooms.AddRange(new List<TourRoom>
                    {
                        new TourRoom
                        {
                            TourId = haNoiTour.Id,
                            RoomId = 1,
                            CreatedAt = DateTime.UtcNow
                        },
                        new TourRoom
                        {
                            TourId = haLongTour.Id,
                            RoomId = 2, 
                            CreatedAt = DateTime.UtcNow
                        },
                        new TourRoom
                        {
                            TourId = daNangTour.Id,
                            RoomId = 3,
                            CreatedAt = DateTime.UtcNow
                        },
                        new TourRoom
                        {
                            TourId = phuQuocTour.Id,
                            RoomId = 4,
                            CreatedAt = DateTime.UtcNow
                        }
                    });
                    await _context.SaveChangesAsync();
                    _logger.Information("Seeded TourRoom.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while seeding the database: {ex.Message}");
            }
        }

        private void SeedScheduleForTour(TourEntity tour)
        {
            _logger.Information($"Seeding Schedule For {tour.Name}");
            var dateFrom = tour.DateFrom;
            var dateTo = tour.DateTo;
            while (dateFrom <= dateTo)
            {
                var dateStart = dateFrom;
                dateFrom = dateFrom.AddDays(tour.DayList?.Count() ?? 0);
                var dateEnd = dateFrom;

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
    }
}
