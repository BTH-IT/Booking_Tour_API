using Shared.Enums;
namespace Shared.DTOs
{
	public class RoomResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<ImagesDTO> Images { get; set; }
		public string? Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public List<ReviewRoom>? Reviews { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
		public int HotelId { get; set; }
		public HotelResponseDTO Hotel { get; set; }
	}

	public class RoomRequestDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<ImagesDTO> Images { get; set; }
		public string? Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public List<ReviewRoom>? Reviews { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
		public int HotelId { get; set; }
	}

	public class RoomSearchRequestDTO
	{
		public string? Name { get; set; }
		public string? Location { get; set; }
		public int? MaxGuests { get; set; }
		public double? MinPrice { get; set; }
		public double? MaxPrice { get; set; }
		public DateTime? CheckIn { get; set; }
		public DateTime? CheckOut { get; set; }
		public string? SortBy { get; set; } = "Price"; 
		public string? SortOrder { get; set; } = "asc"; 
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
