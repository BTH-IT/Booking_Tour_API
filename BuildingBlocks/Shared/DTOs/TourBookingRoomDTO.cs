namespace Shared.DTOs
{
	public class TourBookingRoomResponseDTO
	{
		public int BookingTourId { get; set; }
		public int RoomId { get; set; }
		public double Price { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
	}
	public class TourBookingRoomRequestDTO
	{
		public int RoomId { get; set; }
		public double Price { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
	}
}
