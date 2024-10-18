using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
	[Table("Rooms")]
	public class RoomEntity : EntityBase<int>, IDateTracking
	{
		public required string Name { get; set; }

		[NotMapped]
		public required string[] ImagesList { get; set; }

		[Column(TypeName = "JSON")]
		public string Images
		{
			get => ImagesList == null ? null : JsonConvert.SerializeObject(ImagesList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						ImagesList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						ImagesList = Array.Empty<string>();
					}
				}
				else
				{
					ImagesList = Array.Empty<string>();
				}
			}
		}
		public required string Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }

		[NotMapped]
		public List<ReviewRoom>? ReviewList { get; set; }

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
						ReviewList = JsonConvert.DeserializeObject<List<ReviewRoom>>(value)
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

		[NotMapped]
		public string[]? RoomAmenitiesList { get; set; }

		[Column(TypeName = "JSON")]
		public string? RoomAmenities
		{
			get => RoomAmenitiesList == null ? null : JsonConvert.SerializeObject(RoomAmenitiesList);
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						RoomAmenitiesList = JsonConvert.DeserializeObject<string[]>(value) ?? Array.Empty<string>();
					}
					catch (JsonException)
					{
						RoomAmenitiesList = Array.Empty<string>();
					}
				}
				else
				{
					RoomAmenitiesList = Array.Empty<string>();
				}
			}
		}
		public int HotelId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
		public Hotel? Hotel { get; set; }
	}
}
