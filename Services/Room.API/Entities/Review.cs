namespace Room.API.Entities
{
    public class Review
    {
		public int Id { get; set; }

		public string Content { get; set; }

		public float Rating { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public int RoomId { get; set; }
		public int UserId { get; set; }
	}
}
