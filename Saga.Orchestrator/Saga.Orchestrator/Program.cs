using Booking.API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.DTOs;
using System.Text;
using EventBus.Masstransit;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Saga.Orchestrator.API.Extensions;
var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    builder.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddConfigurationSettings(builder.Configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    // Configure Route Options 
    builder.Services.Configure<RouteOptions>(cfg => cfg.LowercaseQueryStrings = true);
    // Add CORS
    builder.Services.ConfigureCors(builder.Configuration);
    //Configure authentication
    builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("Jwt:SecretKey").Value
                )
            )
        }
    );
    //Add Swagger Gen
    builder.Services.AddSwaggerGen(
        options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem - Sagas API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new List<string>{}
                }
            });
        }
    );
    //Add GrpcClient
    builder.Services.AddGrpcClients();
    //
    builder.Services.AddHttpContextAccessor();
    //
    builder.Services.AddInfrastructures();
    // Configure the HTTP request pipeline.
    var app = builder.Build();
    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("HostAbortedException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}



