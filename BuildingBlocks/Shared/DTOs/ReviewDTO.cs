namespace Shared.DTOs
{
	public class ReviewRoomDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
	}

	public class ReviewHotelDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
	}
}
