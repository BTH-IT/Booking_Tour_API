using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("tours")]
    public class TourEntity : EntityBase<int>, IDateTracking
    {
        [MaxLength(255)] // Set the maximum length based on your requirements
        public string Name { get; set; }

        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }

        // Adjust the length as needed
        [MaxLength(1000)]
        public string Detail { get; set; }

        // Adjust the length as needed
        [MaxLength(1000)]
        public string Expect { get; set; }

        [Column(TypeName = "decimal(10,2)")] 
        public decimal Price { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }

        // Adjust the length as needed
        [MaxLength(1000)]
        public string Video { get; set; }

        public float SalePercent { get; set; }

        [NotMapped]
        public string[] PriceExcludeList { get; set; }
        [NotMapped]
        public string[] PriceIncludeList { get; set; }
        [NotMapped]
        public string[] ActivityList { get; set; }
        [NotMapped]
        public string[] ImageList { get; set; }
        [NotMapped]
        public Day[] DayList { get; set; }
        [NotMapped]
        public Review[] ReviewList { get; set; }

        public Destination Destination { get; set; }

        [Column(TypeName = "JSON")]
        public string Activities
        {
            get => JsonConvert.SerializeObject(ActivityList);
            set => ActivityList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
        }

        [Column(TypeName = "JSON")]
        public string PriceExcludes
        {
            get => JsonConvert.SerializeObject(PriceExcludeList);
            set => PriceExcludeList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
        }

        [Column(TypeName = "JSON")]
        public string PriceIncludes
        {
            get => JsonConvert.SerializeObject(PriceIncludeList);
            set => PriceIncludeList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
        }

        [Column(TypeName = "JSON")]
        public string Images
        {
            get => JsonConvert.SerializeObject(ImageList);
            set => ImageList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
        }

        [Column(TypeName = "JSON")]
        public string Days
        {
            get => JsonConvert.SerializeObject(DayList);
            set => DayList = JsonConvert.DeserializeObject<Day[]>(value) ?? Array.Empty<Day>();
        }

        [Column(TypeName = "JSON")]
        public string Reviews
        {
            get => JsonConvert.SerializeObject(ReviewList);
            set => ReviewList = JsonConvert.DeserializeObject<Review[]>(value) ?? Array.Empty<Review>();
        }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        public int DestinationId { get; set; }

        public ICollection<Schedule>? Schedules { get; set; }
    }
}
