using Microsoft.EntityFrameworkCore;
using Room.API.Entities;

namespace Room.API.Persistence
{
    public class RoomDbContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }  
        public RoomDbContext(DbContextOptions<RoomDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomEntity>()
                .HasOne(o => o.Hotel)
                .WithMany(c => c.Rooms)
                .HasForeignKey(o => o.HotelId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
