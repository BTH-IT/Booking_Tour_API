using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
    [Table("Rooms")]
    public class RoomEntity : EntityBase<int>, IDateTracking
    {
        public string Name { get; set; }    
        public RoomType Type { get; set; }  
        public BedType BedType { get; set; }
        public double? Rate {  get; set; }   
        public string? Video { get; set; }
        public string? Detail { get; set; }
        public double Price { get;set; }
        public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public int Size { get; set; }
		[NotMapped]
		public Review[] ReviewList { get; set; }
		[NotMapped]
		public RoomAmenities[] RoomAmenitiesList { get; set; }
		[NotMapped]
		public HotelRules[] HotelRulesList { get; set; }
		[NotMapped]
		public HotelAmenities[] HotelAmenitiesList { get; set; }
		public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        public int HotelId {  get; set; }   
        public Hotel? Hotel { get; set; }
		[Column(TypeName = "JSON")]
		public string Reviews
		{
			get => JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = JsonConvert.DeserializeObject<Review[]>(value) ?? Array.Empty<Review>();
		}
		[Column(TypeName = "JSON")]
		public string RoomAmenities
		{
			get => JsonConvert.SerializeObject(RoomAmenitiesList);
			set => RoomAmenitiesList = JsonConvert.DeserializeObject<RoomAmenities[]>(value) ?? Array.Empty<RoomAmenities>();
		}
		[Column(TypeName = "JSON")]
		public string HotelRules
		{
			get => JsonConvert.SerializeObject(HotelRulesList);
			set => HotelRulesList = JsonConvert.DeserializeObject<HotelRules[]>(value) ?? Array.Empty<HotelRules>();
		}
		[Column(TypeName = "JSON")]
		public string HotelAmenities
		{
			get => JsonConvert.SerializeObject(HotelAmenitiesList);
			set => HotelAmenitiesList = JsonConvert.DeserializeObject<HotelAmenities[]>(value) ?? Array.Empty<HotelAmenities>();
		}
	}
}
