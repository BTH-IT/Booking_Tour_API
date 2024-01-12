using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BookingApi.Models
{
    public class Day
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    [Table("Tour")]
    public class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [Column(TypeName = "decimal(10,2)")] // Adjust precision and scale as needed
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

        // Ignore Activities property in database mapping
        [NotMapped]
        public string[] ActivityList { get; set; }

        [NotMapped]
        public string[] ImageList { get; set; }
        [NotMapped]
        public Day[] DayList { get; set; }

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

        public ICollection<Review> Reviews { get; set; }
    }
}
