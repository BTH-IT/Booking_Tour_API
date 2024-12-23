﻿namespace Shared.DTOs
{
	public class BookingRoomResponseDTO
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public DateTime? CheckIn { get; set; }
		public DateTime? CheckOut { get; set; }
		public int NumberOfPeople { get; set; }
		public double PriceTotal { get; set; }
		public string Status { get; set; } = string.Empty;
		public DateTime? CreatedAt {  get; set; }
        public List<DetailBookingRoomResponseDTO> DetailBookingRooms { get; set; }
		public UserResponseDTO User { get; set; }

	}
	public class UpdateBookingStatusDTO
	{
		public string Status { get; set; }
	}
	public class BookingRoomRequestDTO
	{
		public int UserId { get; set; }
		public DateTime? CheckIn { get; set; }
		public DateTime? CheckOut { get; set; }
		public int NumberOfPeople { get; set; }
		public double PriceTotal { get; set; }
		public List<DetailBookingRoomRequestDTO> DetailBookingRooms { get; set; }
	}
	public class RoomBookingDataDTO
	{
		public List<DetailRoomBookingDateDTO> Data { get; set; }
	}
	public class DetailRoomBookingDateDTO
	{
		public DateTime CheckIn { get; set; }
		public DateTime CheckOut { get; set; }
	} 
}
