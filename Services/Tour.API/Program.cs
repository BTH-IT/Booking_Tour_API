using Room.API.Persistence;
using Serilog;
<<<<<<< HEAD
using Tour.API.Extensions;
using Tour.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
=======
using Tour.API;
using Tour.API.Extensions;
using Tour.API.Persistence;
using FluentValidation;
using Tour.API.Entities;
using Tour.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Tour.API.GrpcServer.Services;
using Contracts.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventBus.Masstransit;
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    builder.AddAppConfigurations();

    // Add services to the container.
    builder.Services.AddControllers();
<<<<<<< HEAD
    builder.Services.AddConfigurationSettings(builder.Configuration);
=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add Auto Mapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // Add Fluent Validator 
    builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<TourValidator>());

    // Suppress model state invalid filter if needed
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Add DbContext
    builder.Services.ConfigureIdentityDbContext();

    //Add Cors
    builder.Services.ConfigureCors(builder.Configuration);

    // Add Infrastructure Services
    builder.Services.AddInfrastructureServices();

    // Configure Route Options 
    builder.Services.Configure<RouteOptions>(cfg => cfg.LowercaseQueryStrings = true);
<<<<<<< HEAD
    // Add Masstransit
    builder.Services.AddCustomMassTransit(builder.Environment, typeof(Program).Assembly);
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
=======

    // Add Grpc
    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<GrpcExceptionInterceptor>(); 
    });
    builder.WebHost.ConfigureKestrel(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.ListenAnyIP(5004);
            options.ListenAnyIP(5104, listenOptions =>
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
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    //Add Swagger Gen
    builder.Services.AddSwaggerGen(
        options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem - Tour API", Version = "v1" });
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
<<<<<<< HEAD
    //Add Grpc
    builder.WebHost.ConfigureKestrel(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.ListenAnyIP(5004);
            options.ListenAnyIP(5104, listenOptions =>
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

    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<GrpcExceptionInterceptor>();
    });
    //Add GrpcClient
    builder.Services.AddGrpcClients();

=======
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    // Configure the HTTP request pipeline.
    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    //app.UseHttpsRedirection(); // Uncomment if you want to enable HTTPS redirection
<<<<<<< HEAD
=======
    app.UseAuthentication(); 
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
    app.UseAuthorization();
    app.MapControllers();
    app.MapGrpcService<TourProtoService>();
    // Seeding database async
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<TourDbContextSeeder>();
        await seeder.InitialiseAsync();
        await seeder.SeedAsync();
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
