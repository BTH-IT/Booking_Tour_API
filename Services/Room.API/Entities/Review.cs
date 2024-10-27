namespace Room.API.Entities
{
    public class ReviewRoom
	{
		public string? Id { get; set; }
		public required string Content { get; set; }
		public float Rating { get; set; }
		public int RoomId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}

	public class ReviewHotel
	{
		public string? Id { get; set; }
		public required string Content { get; set; }
		public float Rating { get; set; }
		public int HotelId { get; set; }
		public int UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public DateTime? DeletedAt { get; set; }
	}
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
}
