using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
	[Table("Hotels")]
	public class Hotel : EntityBase<int>, IDateTracking
	{
		public string Name { get; set; }
		public string Location { get; set; }		
		public int LocationCode { get; set; }
		public string Description { get; set; }		
		public string ContactInfo { get; set; }		
		[NotMapped]
		public List<ReviewHotel> ReviewList { get; set; }
		[NotMapped]
		public List<HotelRules> HotelRulesList { get; set; }
		[NotMapped]
		public List<HotelAmenities> HotelAmenitiesList { get; set; }
		[NotMapped]
		public ICollection<RoomEntity> Rooms { get; set; }
		public DateTime CreatedAt { get; set; }		
		public DateTime? UpdatedAt { get; set; }		
		public DateTime? DeletedAt { get; set; }
		[Column(TypeName = "JSON")]
		public string? Reviews
		{
			get => ReviewList == null ? null : JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = value == null ? null : JsonConvert.DeserializeObject<List<ReviewHotel>>(value).Where(r => r.DeletedAt == null).ToList();
		}
		[Column(TypeName = "JSON")]
		public string? HotelRules
		{
			get => HotelRulesList == null ? null : JsonConvert.SerializeObject(HotelRulesList);
			set => HotelRulesList = value == null ? null : JsonConvert.DeserializeObject<List<HotelRules>>(value).ToList();
		}
		[Column(TypeName = "JSON")]
		public string? HotelAmenities
		{
			get => HotelAmenitiesList == null ? null : JsonConvert.SerializeObject(HotelAmenitiesList);
			set => HotelAmenitiesList = value == null ? null : JsonConvert.DeserializeObject<List<HotelAmenities>>(value).ToList();
		}
	}
}
