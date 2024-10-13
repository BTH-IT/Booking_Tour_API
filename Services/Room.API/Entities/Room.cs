using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
    [Table("Rooms")]
    public class RoomEntity : EntityBase<int>, IDateTracking
    {
        public string Name { get; set; }
		[NotMapped]
		public List<Image> ImagesList { get; set; }
		[NotMapped]
		public Video VideoObject { get; set; }
		public string? Detail { get; set; }
        public double Price { get;set; }
        public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		[NotMapped]
		public List<ReviewRoom> ReviewList { get; set; }
		[NotMapped]
		public List<RoomAmenities> RoomAmenitiesList { get; set; }

		public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        public int HotelId {  get; set; }
		public DateTime? DeletedAt { get; set; }
		public Hotel? Hotel { get; set; }
		[Column(TypeName = "JSON")]
		public string Reviews
		{
			get => JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = JsonConvert.DeserializeObject<List<ReviewRoom>>(value).Where(r => r.DeletedAt == null).ToList() ?? new List<ReviewRoom>();

		}
		[Column(TypeName = "JSON")]
		public string RoomAmenities
		{
			get => JsonConvert.SerializeObject(RoomAmenitiesList);
			set => RoomAmenitiesList = JsonConvert.DeserializeObject<List<RoomAmenities>>(value) ?? new List<RoomAmenities>();

		}
		[Column(TypeName = "JSON")]
		public string Images
		{
			get => JsonConvert.SerializeObject(ImagesList);
			set => ImagesList = JsonConvert.DeserializeObject<List<Image>>(value) ?? new List<Image>();
		}
		[Column(TypeName = "JSON")]
		public string Video
		{
			get => JsonConvert.SerializeObject(VideoObject);
			set => VideoObject  = JsonConvert.DeserializeObject<Video>(value) ?? new Video();
		}
	}
}
