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
        public string? Name { get; set; }

        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }

        [MaxLength(1000)] // Đặt chiều dài tối đa cho chi tiết tour
        public string? Detail { get; set; }

        [MaxLength(1000)] // Đặt chiều dài tối đa cho kỳ vọng
        public string Expect { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }

		[NotMapped]
		public Video? VideoObject { get; set; }

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
        public DateTime[] DayList { get; set; }

        [NotMapped]
		public List<Review> ReviewList { get; set; }
		public int DestinationId { get; set; } // Khóa ngoại đến Destination
        [ForeignKey(nameof(DestinationId))]
        public virtual DestinationEntity Destination { get; set; } // Thêm mối quan hệ với Destination

        // Danh sách lịch trình cho tour
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); // Khởi tạo danh sách

		[Column(TypeName = "JSON")]
		public string? Video
		{
			get => VideoObject == null ? null : JsonConvert.SerializeObject(VideoObject);
			set => VideoObject = value == null ? null : JsonConvert.DeserializeObject<Video>(value);
		}

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
            get => JsonConvert.SerializeObject(DayList.Select(d => new { Date = d }));
            set
            {
                try
                {
                    var daysList = JsonConvert.DeserializeObject<List<Dictionary<string, DateTime>>>(value);
                    DayList = daysList?.Select(d => d["Date"]).ToArray() ?? Array.Empty<DateTime>();
                }
                catch (JsonException)
                {
                    DayList = Array.Empty<DateTime>(); 
                }
            }
        }


        [Column(TypeName = "JSON")]
        public string? Reviews
        {
			get => ReviewList == null ? null : JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = value == null ? null : JsonConvert.DeserializeObject<List<Review>>(value).Where(r => r.DeletedAt == null).ToList();
		}

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
