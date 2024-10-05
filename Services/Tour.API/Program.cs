﻿using Room.API.Persistence;
using Serilog;
using Tour.API;
using Tour.API.Extensions;
using Tour.API.Persistence;
using FluentValidation;
using Tour.API.Entities;
using Tour.API.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    builder.AddAppConfigurations();

    // Add services to the container.
    builder.Services.AddControllers();

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

    // Configure the HTTP request pipeline.
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors("CorsPolicy");
    // app.UseHttpsRedirection(); // Uncomment if you want to enable HTTPS redirection

    app.UseAuthorization();
    app.MapControllers();

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
