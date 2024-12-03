namespace Shared.DTOs
{
	public class RoomResponseDTO
	{
		public int Id { get; set; }
<<<<<<< HEAD
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
=======
		public string Name { get; set; }
		public List<ImagesDTO> Images { get; set; }
		public VideosDTO? Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; }
		public int MaxGuests { get; set; }
		public List<ReviewRoomDTO>? Reviews { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
		public int HotelId { get; set; }
		public HotelResponseDTO Hotel { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}

	public class RoomRequestDTO
	{
<<<<<<< HEAD
		public required string Name { get; set; }
		public required string[] Images { get; set; }
		public required string Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool IsAvailable { get; set; } = true;
		public int MaxGuests { get; set; }
		public string[]? RoomAmenities { get; set; }
=======
		public string Name { get; set; }
		public List<ImagesDTO> Images { get; set; }
		public VideosDTO Video { get; set; }
		public string? Detail { get; set; }
		public double Price { get; set; }
		public bool? IsAvailable { get; set; } = true;
		public int MaxGuests { get; set; }
		public List<RoomAmenitiesDTO>? RoomAmenities { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public int HotelId { get; set; }
	}

	public class RoomSearchRequestDTO
	{
		public string? Name { get; set; }
<<<<<<< HEAD
		public string[]? LocationCode { get; set; }
=======
		public string? Location { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public int? MaxGuests { get; set; }
		public double? MinPrice { get; set; }
		public double? MaxPrice { get; set; }
		public DateTime? CheckIn { get; set; }
		public DateTime? CheckOut { get; set; }
<<<<<<< HEAD
        public string[]? HotelRules { get; set; }
        public string[]? HotelAmenities { get; set; }
        public string[]? RoomAmenities { get; set; }
        public string? SortBy { get; set; } = "Price"; 
=======
		public string? SortBy { get; set; } = "Price"; 
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public string? SortOrder { get; set; } = "asc"; 
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}

<<<<<<< HEAD
	public class RoomSearchResponseDTO
    {
		public required List<RoomResponseDTO> Rooms { get; set; }
		public required int TotalItems { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public required int PageNumber { get; set; }
		public required int PageSize { get; set; }
=======
	public class PagedRoomResponseDTO
	{
		public List<RoomResponseDTO> Rooms { get; set; }
		public int TotalItems { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
