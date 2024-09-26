using Shared.Enums;
namespace Shared.DTOs
{
	public class RoomResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public RoomType Type { get; set; }
		public BedType BedType { get; set; }
		public double? Rate { get; set; }
		public string? Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public int Size { get; set; }
		public List<ReviewRoom>? Reviews { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int HotelId { get; set; }
		public HotelRulesResponseDTO? Hotel { get; set; }
		public DateTime? DeletedAt { get; set; }
	}

	public class RoomRequestDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public RoomType Type { get; set; }
		public BedType BedType { get; set; }
		public double? Rate { get; set; }
		public string? Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public int Size { get; set; }
		public List<ReviewRoom>? Reviews { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int HotelId { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
