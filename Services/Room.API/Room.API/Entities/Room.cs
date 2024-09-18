using Contracts.Domains;
using Contracts.Domains.Interfaces;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.API.Entities
{
    [Table("Rooms")]
    public class RoomEntity : EntityBase<int>, IDateTracking
    {
        public string Name { get; set; }    
        public RoomType Type { get; set; }  
        public BedType BedType { get; set; }
        public double? Rate {  get; set; }   
        public string? Video { get; set; }
        public string? Detail { get; set; }
        public double Price { get;set; }
        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
        // Foreign Key To Hotel
        public int HotelId {  get; set; }   
        public Hotel? Hotel { get; set; }   
    }
}
