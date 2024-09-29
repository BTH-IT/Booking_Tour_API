﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
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