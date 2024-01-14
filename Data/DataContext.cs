using BookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleDetail>()
                .HasOne(roleDetail => roleDetail.Role)
                .WithMany(role => role.RoleDetails);

            modelBuilder.Entity<RoleDetail>()
                .HasOne(roleDetail => roleDetail.Permission)
                .WithMany(permission => permission.RoleDetails);

            modelBuilder.Entity<Schedule>()
                .HasOne(schedule => schedule.Tour)
                .WithMany(tour => tour.Schedules);

            modelBuilder.Entity<RoleDetail>()
                .HasKey(roleDetail => new { roleDetail.RoleId, roleDetail.PermissionId, roleDetail.ActionName });

            base.OnModelCreating(modelBuilder);
        }

        #region DbSet
        public DbSet<Tour>? Tours { get; set; }
        public DbSet<Destination>? Destinations { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<RoleDetail>? RoleDetails { get; set; }
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<BookingTour>? BookingTours { get; set; }
        public DbSet<Schedule>? Schedules { get; set; }
        public DbSet<Permission>? Permissions { get; set; }
        #endregion
    }
}
