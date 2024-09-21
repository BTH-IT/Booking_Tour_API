using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("tours")]
    public class TourEntity : EntityBase<int>, IDateTracking
    {
        [MaxLength(255)] // Đặt chiều dài tối đa cho tên tour
        public string Name { get; set; }

        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }

        [MaxLength(1000)] // Đặt chiều dài tối đa cho chi tiết tour
        public string Detail { get; set; }

        [MaxLength(1000)] // Đặt chiều dài tối đa cho kỳ vọng
        public string Expect { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }

        [MaxLength(1000)] // Đặt chiều dài tối đa cho video
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

        public int DestinationId { get; set; } // Khóa ngoại đến Destination
        [ForeignKey(nameof(DestinationId))]
        public virtual DestinationEntity Destination { get; set; } // Thêm mối quan hệ với Destination

        // Danh sách lịch trình cho tour
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); // Khởi tạo danh sách

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

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
