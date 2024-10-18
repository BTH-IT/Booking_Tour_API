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
		public string? Reviews
		{
			get => ReviewList == null ? null : JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = value == null ? null : JsonConvert.DeserializeObject<List<ReviewRoom>>(value).Where(r => r.DeletedAt == null).ToList();
		}

		[Column(TypeName = "JSON")]
		public string? RoomAmenities
		{
			get => RoomAmenitiesList == null ? null : JsonConvert.SerializeObject(RoomAmenitiesList);
			set => RoomAmenitiesList = value == null ? null : JsonConvert.DeserializeObject<List<RoomAmenities>>(value).ToList();
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
