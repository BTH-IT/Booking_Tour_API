using Identity.API.Entites;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Persistence
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
        #region DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleDetail> RoleDetails { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Identity.API.Entites.Account> Accounts {get;set;}

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region n_n_relation_role_permission
            modelBuilder.Entity<RoleDetail>()
                .HasOne(roleDetail => roleDetail.Role)
                .WithMany(role => role.RoleDetails);

            modelBuilder.Entity<RoleDetail>()
                .HasOne(roleDetail => roleDetail.Permission)
                .WithMany(permission => permission.RoleDetails);

            modelBuilder.Entity<RoleDetail>()
                .HasKey(roleDetail => new { roleDetail.RoleId, roleDetail.PermissionId, roleDetail.ActionName});
            #endregion

            #region 1_1_relation_user_account
            modelBuilder.Entity<User>()
                .HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<User>(u => u.AccountId)
                .IsRequired();
            #endregion

            #region n_1_relation_account_role
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.RoleId);
            #endregion
            base.OnModelCreating(modelBuilder);
        }
    }
}
