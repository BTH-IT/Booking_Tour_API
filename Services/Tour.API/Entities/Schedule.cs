using Contracts.Domains;
using Contracts.Domains.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour.API.Entities
{
    [Table("schedules")]
    public class Schedule : EntityBase<int>, IDateTracking
    {

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int AvailableSeats { get; set; }

        //Foreign Key
        public int TourId { get; set; }
        public TourEntity? Tour { get; set; }
        public DateTime CreatedAt { get ; set ; }
        public DateTime? UpdatedAt { get ; set ; }
    }
}
