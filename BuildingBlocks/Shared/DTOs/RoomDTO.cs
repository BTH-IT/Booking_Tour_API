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
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int HotelId { get; set; }
		public HotelRulesResponseDTO? HotelRules { get; set; }
		public HotelAmenitiesResponseDTO? HotelAmenities { get; set; }
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
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int HotelId { get; set; }
		public DateTime? DeletedAt { get; set; }
	}

	public class RoomSearchRequestDTO
	{
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
		public int Rooms { get; set; }
		public GuestsDTO Guests { get; set; }
		public List<string> Facilities { get; set; }
		public string RoomSize { get; set; }
	}

	public class GuestsDTO
	{
		public int Adults { get; set; }
		public int Children { get; set; }
	}
}
