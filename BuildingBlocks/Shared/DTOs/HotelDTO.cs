namespace Shared.DTOs
{
	public class HotelResponseDTO
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Location { get; set; }
		public required int LocationCode { get; set; }
		public required string Description { get; set; }
		public required string ContactInfo { get; set; }
		public List<ReviewHotelDTO>? Reviews { get; set; }
		public string[]? HotelRules { get; set; }
		public string[]? HotelAmenities { get; set; }
	}

	public class HotelRequestDTO
	{
		public required string Name { get; set; }
		public required string Location { get; set; }
		public required int LocationCode { get; set; }
		public required string Description { get; set; }
		public required string ContactInfo { get; set; }
		public string[]? HotelRules { get; set; }
		public string[]? HotelAmenities { get; set; } 
	}
}
