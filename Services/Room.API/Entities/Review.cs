namespace Room.API.Entities
{
    public class ReviewRoom
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
	public class ReviewHotel
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
}
