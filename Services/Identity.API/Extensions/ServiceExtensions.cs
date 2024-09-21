using Contracts.Domains.Interfaces;
using Identity.API.Persistence;
using Identity.API.Repositories;
using Identity.API.Repositories.Interfaces;
using Identity.API.Services;
using Identity.API.Services.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Shared.Configurations;

namespace Identity.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureIdentityDbContext(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configured.");
            Console.WriteLine(databaseSettings.ConnectionString);
            var builder = new MySqlConnectionStringBuilder(databaseSettings.ConnectionString);
            services.AddDbContext<IdentityDbContext>(option => option.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString)));

            return services; 
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            return services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
                    .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                    .AddScoped<IPermissionRepository, PermissionRepository>()
                    .AddScoped<IAccountRepository, AccountRepository>()
                    .AddScoped<IUserRepository, UserRepository>()
                    .AddScoped<IRoleRepository, RoleRepositoy>()
                    .AddScoped<IAccountService, AccountService>()
                    .AddScoped<IPermissionService, PermissionService>()
                    .AddScoped<IUserService, UserService>() 
                    .AddScoped<IRoleService, RoleService>()
                    .AddScoped<IAuthService,AuthService>()
                    .AddScoped<IdentityDbContextSeeder>();
                
        }
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(option =>
                option.AddPolicy("CorsPolicy", cfg =>
                    cfg.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                )
            );
            return services;
        }
    }
}
