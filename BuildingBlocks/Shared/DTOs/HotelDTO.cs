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
		public List<ReviewDTO>? Reviews { get; set; }

		public IEnumerable<RoomResponseDTO> Rooms { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}

	public class HotelRequestDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string ContactInfo { get; set; }
		public double? Rate { get; set; }
		public List<ReviewDTO>? Reviews { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
