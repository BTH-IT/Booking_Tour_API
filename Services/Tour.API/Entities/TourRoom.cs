using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Contracts.Domains;

namespace Tour.API.Entities
{
    [Table("tour_rooms")]
    public class TourRoom : EntityBase<int>
    {
        public int TourId { get; set; }
        public int RoomId { get; set; }
        public TourEntity? Tour { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
