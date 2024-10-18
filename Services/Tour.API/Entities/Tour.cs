using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("tours")]

    public class TourEntity : EntityBase<int>, IDateTracking
    {
        [MaxLength(255)] 
        public string? Name { get; set; }
        public int MaxGuests { get; set; }
        public bool IsWifi { get; set; }
        [MaxLength(1000)] 
        public string? Detail { get; set; }
        [MaxLength(1000)]
        public required string Expect { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float Rate { get; set; }
		public required string Video { get; set; }
        public float SalePercent { get; set; }
        [NotMapped]
        public string[]? PriceExcludeList { get; set; }
        [NotMapped]
        public string[]? PriceIncludeList { get; set; }
        [NotMapped]
        public string[]? ActivityList { get; set; }
        [NotMapped]
        public string[]? ImageList { get; set; }
        [NotMapped]
        public string[]? DayList { get; set; }
        [NotMapped]
		public List<Review>? ReviewList { get; set; }
		public int DestinationId { get; set; } 
        [ForeignKey(nameof(DestinationId))]
        public virtual DestinationEntity Destination { get; set; }
		public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>(); 
		[Column(TypeName = "JSON")]
		public string? Activities
		{
			get => ActivityList == null ? null : JsonConvert.SerializeObject(ActivityList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						ActivityList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						ActivityList = Array.Empty<string>();
					}
				}
				else
				{
					ActivityList = Array.Empty<string>();
				}
			}
		}
		[Column(TypeName = "JSON")]
		public string? PriceExcludes
		{
			get => PriceExcludeList == null ? null : JsonConvert.SerializeObject(PriceExcludeList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						PriceExcludeList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						PriceExcludeList = Array.Empty<string>();
					}
				}
				else
				{
					PriceExcludeList = Array.Empty<string>();
				}
			}
		}
		[Column(TypeName = "JSON")]
		public string? PriceIncludes
		{
			get => PriceIncludeList == null ? null : JsonConvert.SerializeObject(PriceIncludeList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						PriceIncludeList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						PriceIncludeList = Array.Empty<string>();
					}
				}
				else
				{
					PriceIncludeList = Array.Empty<string>();
				}
			}
		}
		[Column(TypeName = "JSON")]
		public string? Images
		{
			get => ImageList == null ? null : JsonConvert.SerializeObject(ImageList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						ImageList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						ImageList = Array.Empty<string>();
					}
				}
				else
				{
					ImageList = Array.Empty<string>();
				}
			}
		}
		[Column(TypeName = "JSON")]
		public string? Days
		{
			get => DayList == null ? null : JsonConvert.SerializeObject(DayList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						DayList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						DayList = Array.Empty<string>();
					}
				}
				else
				{
					DayList = Array.Empty<string>();
				}
			}
		}
		[Column(TypeName = "JSON")]
		public string? Reviews
		{
			get => ReviewList == null ? null : JsonConvert.SerializeObject(ReviewList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						ReviewList = JsonConvert.DeserializeObject<List<Review>>(value)?.Where(r => r.DeletedAt == null).ToList() ?? new List<Review>();
					}
					catch (JsonException)
					{
						ReviewList = new List<Review>();
					}
				}
				else
				{
					ReviewList = new List<Review>();
				}
			}
		}
		public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
