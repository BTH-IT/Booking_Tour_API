﻿namespace Shared.DTOs
{
	public class ReviewRoomDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt {  get; set; }
	}

	public class ReviewHotelDTO
	{
		public string Id { get; set; }
		public string Content { get; set; }
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
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
	}
}
