namespace Shared.DTOs
{
	public class ReviewRoomDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
<<<<<<< HEAD
		public DateTime? CreatedAt {  get; set; }
	}

=======
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	public class ReviewHotelDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
<<<<<<< HEAD
		public DateTime? CreatedAt {  get; set; }
	}

	public class ReviewTourDTO
	{
		public string Id { get; set; }
		public string? Content { get; set; }
		public float Rating { get; set; }
		public int TourId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt {  get; set; }
=======
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	}
}
