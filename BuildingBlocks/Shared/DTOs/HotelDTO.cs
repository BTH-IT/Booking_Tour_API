using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
	public class HotelResponseDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public double? Rate { get; set; }
		public List<ReviewHotel>? Reviews { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public IEnumerable<RoomResponseDTO> Rooms { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
	public class HotelRulesResponseDTO
	{
		public int Id { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
	}

	public class HotelRequestDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public double? Rate { get; set; }
		public List<ReviewHotel>? Reviews { get; set; }
		public List<HotelRulesDTO>? HotelRules { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
