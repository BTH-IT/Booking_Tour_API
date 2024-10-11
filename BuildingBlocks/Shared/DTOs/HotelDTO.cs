namespace Shared.DTOs
{
	public class HotelResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public int LocationCode { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public List<ReviewHotelDTO>? Reviews { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}

	public class HotelRequestDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public int LocationCode { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public List<ReviewHotelDTO>? Reviews { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public List<HotelAmenitiesDTO>? HotelAmenities { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
