using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
	[Table("Hotels")]
	public class Hotel : EntityBase<int>, IDateTracking
	{
		public required string Name { get; set; }
		public required string Location { get; set; }
		public required int LocationCode { get; set; }
		public required string Description { get; set; }
		public required string ContactInfo { get; set; }

		[NotMapped]
		public List<ReviewHotel>? ReviewList { get; set; }

		[NotMapped]
		public string[]? HotelRulesList { get; set; }

		[NotMapped]
		public string[]? HotelAmenitiesList { get; set; }

		[Column(TypeName = "JSON")]
		public string? Reviews
		{
			get
			{
				if (ReviewList == null)
					return null;

				var filteredReviews = ReviewList.Where(r => r.DeletedAt == null);
				return JsonConvert.SerializeObject(filteredReviews);
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						ReviewList = JsonConvert.DeserializeObject<List<ReviewHotel>>(value)
										?.Where(r => r.DeletedAt == null)
										.ToList();
					}
					catch (JsonException)
					{
						ReviewList = null;
					}
				}
				else
				{
					ReviewList = null;
				}
			}
		}

		[Column(TypeName = "JSON")]
		public string? HotelRules
		{
			get => HotelRulesList == null ? null : JsonConvert.SerializeObject(HotelRulesList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						HotelRulesList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						HotelRulesList = Array.Empty<string>();
					}
				}
				else
				{
					HotelRulesList = Array.Empty<string>();
				}
			}
		}

		[Column(TypeName = "JSON")]
		public string? HotelAmenities
		{
			get => HotelAmenitiesList == null ? null : JsonConvert.SerializeObject(HotelAmenitiesList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						HotelAmenitiesList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						HotelAmenitiesList = Array.Empty<string>();
					}
				}
				else
				{
					HotelAmenitiesList = Array.Empty<string>();
				}
			}
		}

		[NotMapped]
		public ICollection<RoomEntity>? Rooms { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
