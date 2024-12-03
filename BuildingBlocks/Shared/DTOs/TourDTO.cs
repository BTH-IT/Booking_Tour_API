namespace Shared.DTOs
{
    public class TourRequestDTO
    {
<<<<<<< HEAD
=======
        public int? Id { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public string? Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        public string? Detail { get; set; }
        public string? Expect { get; set; }
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
<<<<<<< HEAD
        public required string Video { get; set; }
=======
        public string? Video { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public float SalePercent { get; set; }
        public string[]? PriceExcludeList { get; set; }
        public string[]? PriceIncludeList { get; set; }
        public string[]? ActivityList { get; set; }
        public string[]? ImageList { get; set; }
<<<<<<< HEAD
        public string[]? DayList { get; set; }
        public int DestinationId { get; set; }
=======
        public List<DateTime>? DayList { get; set; }
        public int DestinationId { get; set; }
        public List<Review>? ReviewList { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
    public class Review
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public float Rating { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int TourId { get; set; }
        public int UserId { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
    public class TourResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        public string? Detail { get; set; }
        public string? Expect { get; set; }
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
<<<<<<< HEAD
        public required string Video { get; set; }
=======
        public string? Video { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public float SalePercent { get; set; }
        public string[]? PriceExcludeList { get; set; }
        public string[]? PriceIncludeList { get; set; }
        public string[]? ActivityList { get; set; }
        public string[]? ImageList { get; set; }
<<<<<<< HEAD
        public string[]? DayList { get; set; }
		public int DestinationId { get; set; }
        public DestinationResponseDTO? Destination { get; set; } 
        public List<ReviewTourDTO>? ReviewList { get; set; } = new List<ReviewTourDTO>();
    }
    public class TourCustomResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        public string? Detail { get; set; }
        public string? Expect { get; set; }
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
        public required string Video { get; set; }
        public float SalePercent { get; set; }
        public string[]? PriceExcludeList { get; set; }
        public string[]? PriceIncludeList { get; set; }
        public string[]? ActivityList { get; set; }
        public string[]? ImageList { get; set; }
        public string[]? DayList { get; set; }
        public int DestinationId { get; set; }

    }

    public class TourSearchResponseDTO
    {
        public required List<TourResponseDTO> Tours { get; set; }
=======
        public List<DateTime>? DayList { get; set; }
        public DestinationRequestDTO? Destination { get; set; }
        public List<Review>? ReviewList { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
    public class TourSearchResponseDTO
    {
        public List<TourResponseDTO> Tours { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
    public class TourSearchRequestDTO
    {
        public string? Keyword { get; set; } 
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public double? Rating { get; set; } 
        public List<string>? Activities { get; set; } 
        public List<string>? Destinations { get; set; }
        public string? SortBy { get; set; } = "releasedate";
        public bool IsDescending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
<<<<<<< HEAD
        public int PageSize { get; set; } = 10; 
=======
        public int PageSize { get; set; } = 2; 
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    }
}
