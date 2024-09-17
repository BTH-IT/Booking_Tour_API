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
        /* Lỗi fix sau */
        //public List<Day> DayList { get; set; }
        public int DestinationId { get; set; }
        /* Lỗi fix sau */
        //public List<Review> ReviewList { get; set; }
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
        /* Lỗi fix sau */
        //public List<Day> DayList { get; set; }
        /* Lỗi fix sau */
        //public Destination Destination { get; set; }
        /* Lỗi fix sau */
        //public List<Review> ReviewList { get; set; }
    }
}
