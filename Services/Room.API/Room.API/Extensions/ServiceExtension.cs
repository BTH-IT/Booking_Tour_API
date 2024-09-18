using Contracts.Domains.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Room.API.Persistence;
using Shared.Configurations;
using Infrastructure.Extensions;
namespace Room.API.Extensions
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
            services.AddDbContext<RoomDbContext>(option => option.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString)));

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<RoomDbContextSeeder>();

            return services;
        }
    }
}
