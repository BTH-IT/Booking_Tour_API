namespace Shared.DTOs
{
    public class DetailBookingRoomResponseDTO
	{
		public int Id { get; set; }
		public int BookingId { get; set; }
		public int RoomId { get; set; }
		public double Price { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
		public RoomResponseDTO? Room { get; set; }
	}
	public class DetailBookingRoomRequestDTO
	{
		public int RoomId { get; set; }
		public double Price { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
	}
}
