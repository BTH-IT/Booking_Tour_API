using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleDetail>()
                .HasOne(rd => rd.Role)
                .WithMany(r => r.RoleDetails);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Tour)
                .WithMany(t => t.Reviews);

            modelBuilder.Entity<RoleDetail>()
                .HasKey(rd => new { rd.RoleId, rd.ActionName });

            // Additional configurations or mappings can be added here

            base.OnModelCreating(modelBuilder);
        }

        #region DbSet
        public DbSet<Tour>? Tours { get; set; }
        public DbSet<Destination>? Destinations { get; set; }
        public DbSet<Review>? Reviews { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<RoleDetail>? RoleDetails { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<BookingTour>? BookingTours { get; set; }
        public DbSet<Schedule>? Schedules { get; set; }
        #endregion
    }
}
