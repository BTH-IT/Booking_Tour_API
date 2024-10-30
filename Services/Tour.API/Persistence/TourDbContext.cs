using Microsoft.EntityFrameworkCore;
using Tour.API.Entities;

namespace Tour.API.Persistence
{
    public class TourDbContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<DestinationEntity> Destinations { get; set; }
        public DbSet<TourEntity> Tours { get; set; }
        public DbSet<TourRoom> TourRooms { get; set; }
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 1_n_relation_destinations_tours
            modelBuilder.Entity<TourEntity>()
                .HasOne(c=>c.Destination)
                .WithMany(o=>o.Tours)
                .HasForeignKey(o=>o.DestinationId);
            #endregion
            #region 1_n_relation_tours_schedules
            modelBuilder.Entity<Schedule>()
                .HasOne(c => c.Tour)
                .WithMany(o => o.Schedules)
                .HasForeignKey(c=>c.TourId);
            #endregion
            #region 1_n_relation_tours_tourRooms
            modelBuilder.Entity<TourRoom>()
                .HasOne(tr => tr.Tour)             
                .WithMany(t => t.TourRooms)          
                .HasForeignKey(tr => tr.TourId);
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
