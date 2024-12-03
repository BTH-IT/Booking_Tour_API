namespace Room.API.Entities
{
    public class ReviewRoom
	{
<<<<<<< HEAD
		public string? Id { get; set; }
		public required string Content { get; set; }
=======
		public string Id { get; set; }
		public string Content { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
<<<<<<< HEAD

	public class ReviewHotel
	{
		public string? Id { get; set; }
		public required string Content { get; set; }
=======
	public class ReviewHotel
	{
		public string Id { get; set; }
		public string Content { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
<<<<<<< HEAD
    public class RoomSearchResult
    {
        public List<RoomEntity> Tours { get; set; } = new List<RoomEntity>();
        public int TotalItems { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public RoomSearchResult(List<RoomEntity> tours, int totalItems, decimal minPrice, decimal maxPrice, int pageNumber, int pageSize)
        {
            Tours = tours;
            TotalItems = totalItems;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
}
