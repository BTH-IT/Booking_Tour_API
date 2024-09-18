using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("destinations")]
    public class Destination : EntityBase<int>, IDateTracking
    {
        public string Name {  get; set; }   
        public string? Description { get; set; }
        public string Url { get; set; }
        public ICollection<TourEntity>? Tours { get; set; }    
        public DateTime CreatedAt { get ; set; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
