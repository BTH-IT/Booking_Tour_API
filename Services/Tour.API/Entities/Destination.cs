using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("destinations")]
    public class DestinationEntity : EntityBase<int>, IDateTracking
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Url { get; set; }
<<<<<<< HEAD
=======

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public ICollection<TourEntity>? Tours { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

