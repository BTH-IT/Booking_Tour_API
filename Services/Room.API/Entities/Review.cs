﻿namespace Room.API.Entities
{
    public class ReviewRoom
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public float Rating { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public int RoomId { get; set; }
		public int UserId { get; set; }
	}
	public class ReviewHotel
	{
		public int Id { get; set; }

		public string Content { get; set; }

		public float Rating { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public int HotelId { get; set; }
		public int UserId { get; set; }
	}
}
