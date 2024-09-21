namespace Shared.DTOs
{
    public class TourRequestDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        public string Detail { get; set; }
        public string Expect { get; set; }
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
        public string Video { get; set; }
        public float SalePercent { get; set; }
        public string[] PriceExcludeList { get; set; }
        public string[] PriceIncludeList { get; set; }
        public string[] ActivityList { get; set; }
        public string[] ImageList { get; set; }
        public List<DateTime> DayList { get; set; }
        public int DestinationId { get; set; }
        public List<Review> ReviewList { get; set; }

    }
    public class Review
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public float Rating { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int TourId { get; set; }
        public int UserId { get; set; }
    }
    public class TourResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        public string Detail { get; set; }
        public string Expect { get; set; }
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
        public string Video { get; set; }
        public float SalePercent { get; set; }
        public string[] PriceExcludeList { get; set; }
        public string[] PriceIncludeList { get; set; }
        public string[] ActivityList { get; set; }
        public string[] ImageList { get; set; }
        public List<DateTime> DayList { get; set; }
        public DestinationRequestDTO Destination { get; set; }
        public List<Review> ReviewList { get; set; }
    }
}
