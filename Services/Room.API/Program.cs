using Contracts.Exceptions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
<<<<<<< HEAD
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
=======
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Room.API;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Room.API.Extensions;
using Room.API.GrpcServer.Services;
using Room.API.Persistence;
using Room.API.Validators;
using Serilog;
using System.Text;
<<<<<<< HEAD
using EventBus.Masstransit;
=======
using System.Text.Json.Serialization;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");


try
{
    builder.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddControllers() ;
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
<<<<<<< HEAD
	// Add Auto Mapper
	builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    // Add Fluent Validator 
    builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<HotelRequestDTOValidator>());
=======
    // Add Auto Mapper
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
	// Add Fluent Validator 
	builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<HotelRequestDTOValidator>());
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
	builder.Services.Configure<ApiBehaviorOptions>(options =>
	{
	    options.SuppressModelStateInvalidFilter = true;
	});
    // Add Authentication
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
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem - Room API", Version = "v1" });
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
    // Add DbContext
    builder.Services.ConfigureIdentityDbContext();
    // Add Infrastructure Services
    builder.Services.AddInfrastructureServices();
    // Add Cors
    builder.Services.ConfigureCors(builder.Configuration);
    // Add Grpc
    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<GrpcExceptionInterceptor>();
    });
<<<<<<< HEAD
    // Add Masstransit
    builder.Services.AddCustomMassTransit(builder.Environment, typeof(Program).Assembly);

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    builder.WebHost.ConfigureKestrel(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.ListenAnyIP(5003);
            options.ListenAnyIP(5103, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        }
        else if (builder.Environment.IsEnvironment("docker"))
        {
            options.ListenAnyIP(80);
            options.ListenAnyIP(81, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        }
    });
    // Configure Route Options 
    builder.Services.Configure<RouteOptions>(cfg => cfg.LowercaseQueryStrings = true);
    // Configure the HTTP request pipeline.

    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");
<<<<<<< HEAD
=======
    app.UseAuthentication();
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    app.UseAuthorization();
    app.MapGrpcService<RoomProtoService>();
    app.MapControllers();
    // Seeding database async
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<RoomDbContextSeeder>();
        await seeder.InitialiseAsync();
		await seeder.RoomDbSeedAsync();
    }
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
