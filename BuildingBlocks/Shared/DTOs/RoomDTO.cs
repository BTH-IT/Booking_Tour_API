namespace Shared.DTOs
{
	public class RoomResponseDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string[] Images { get; set; }
		public required string Video { get; set; }
		public string? Detail { get; set; }
		public required double Price { get; set; }
		public required bool IsAvailable { get; set; }
		public required int MaxGuests { get; set; }
		public List<ReviewRoomDTO>? Reviews { get; set; }
		public string[]? RoomAmenities { get; set; }
		public int HotelId { get; set; }
		public required HotelResponseDTO Hotel { get; set; }
	}

	public class RoomRequestDTO
	{
		public required string Name { get; set; }
		public required string[] Images { get; set; }
		public required string Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; } = true;
		public int MaxGuests { get; set; }
		public string[]? RoomAmenities { get; set; }
		public int HotelId { get; set; }
	}

	public class RoomSearchRequestDTO
	{
		public string? Name { get; set; }
		public string[]? LocationCode { get; set; }
		public int? MaxGuests { get; set; }
		public double? MinPrice { get; set; }
		public double? MaxPrice { get; set; }
		public DateTime? CheckIn { get; set; }
		public DateTime? CheckOut { get; set; }
        public string[]? HotelRules { get; set; }
        public string[]? HotelAmenities { get; set; }
        public string[]? RoomAmenities { get; set; }
        public string? SortBy { get; set; } = "Price"; 
		public string? SortOrder { get; set; } = "asc"; 
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}

	public class RoomSearchResponseDTO
    {
		public required List<RoomResponseDTO> Rooms { get; set; }
		public required int TotalItems { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public required int PageNumber { get; set; }
		public required int PageSize { get; set; }
	}
}
