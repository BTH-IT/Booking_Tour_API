using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
    [Table("Hotels")]
    public class Hotel : EntityBase<int>, IDateTracking
    {
        public string Name { get; set; }
        public string Location { get;set; }
        public string Description { get; set; }
        public string ContactInfo { get; set; }
		public double Rate { get; set; }
		[NotMapped]
		public Review[] ReviewList { get; set; }
		[NotMapped]
		public ICollection<RoomEntity> Rooms { get; set; }    
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get; set ; }
		[Column(TypeName = "JSON")]
		public string Reviews
		{
			get => JsonConvert.SerializeObject(ReviewList);
			set => ReviewList = JsonConvert.DeserializeObject<Review[]>(value) ?? Array.Empty<Review>();
		}
	}
}
