﻿using Room.API.Persistence;
using Serilog;
using Tour.API.Extensions;
using Tour.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Tour.API.GrpcServer.Services;
using Contracts.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventBus.Masstransit;

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
	// Add Redis Distributed Caching
	builder.Services.AddStackExchangeRedisCache(options =>
	{
		options.Configuration = "redis-container:6379";
		options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
		{
			AbortOnConnectFail = true,
			EndPoints = { "redis-container:6379" },
			DefaultDatabase = 3 // Use database 3
		};
	});
	builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<GrpcExceptionInterceptor>();
    });
    //Add GrpcClient
    builder.Services.AddGrpcClients();

    // Configure the HTTP request pipeline.
    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    //app.UseHttpsRedirection(); // Uncomment if you want to enable HTTPS redirection
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
