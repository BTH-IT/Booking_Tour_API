namespace Shared.DTOs
{
	public class HotelResponseDTO
	{
		public int Id { get; set; }
<<<<<<< HEAD
		public required string Name { get; set; }
		public required string Location { get; set; }
		public required int LocationCode { get; set; }
		public required string Description { get; set; }
		public required string ContactInfo { get; set; }
		public List<ReviewHotelDTO>? Reviews { get; set; }
		public string[]? HotelRules { get; set; }
		public string[]? HotelAmenities { get; set; }
=======
		public string Name { get; set; }
		public string Location { get; set; }
		public int LocationCode { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public List<ReviewHotelDTO>? Reviews { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}

	public class HotelRequestDTO
	{
<<<<<<< HEAD
		public required string Name { get; set; }
		public required string Location { get; set; }
		public required int LocationCode { get; set; }
		public required string Description { get; set; }
		public required string ContactInfo { get; set; }
		public string[]? HotelRules { get; set; }
		public string[]? HotelAmenities { get; set; } 
=======
		public string Name { get; set; }
		public string Location { get; set; }
		public int LocationCode { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
