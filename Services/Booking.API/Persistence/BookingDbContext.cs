using Booking.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.API.Persistence
{
    public class BookingDbContext : DbContext
    {
        #region db_set
        public DbSet<BookingTour> BookingTours { get; set; }
        public DbSet<BookingRoom> BookingRooms { get; set; }
<<<<<<< HEAD
=======
        public DbSet<TourBookingRoom> TourBookingRooms { get; set; }
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
        public DbSet<DetailBookingRoom> DetailBookingRooms { get; set; }

        #endregion
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailBookingRoom>()
                .HasOne(c => c.BookingRoom)
                .WithMany(e => e.DetailBookingRooms)
                .HasForeignKey(c => c.BookingId);

<<<<<<< HEAD
=======
            modelBuilder.Entity<TourBookingRoom>()
                .HasOne(c => c.BookingTour)
                .WithMany(e => e.TourBookingRooms)
                .HasForeignKey(c=> c.BookingTourId);
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
