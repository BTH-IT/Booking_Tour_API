using Contracts.Exceptions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Room.API;
using Room.API.Extensions;
using Room.API.GrpcServer.Services;
using Room.API.Persistence;
using Room.API.Validators;
using Serilog;
using System.Text.Json.Serialization;

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
    // Add Auto Mapper
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
	// Add Fluent Validator 
	builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<HotelRequestDTOValidator>());
	builder.Services.Configure<ApiBehaviorOptions>(options =>
	{
	    options.SuppressModelStateInvalidFilter = true;
	});
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
    app.UseAuthorization();
    app.MapGrpcService<RoomProtoService>();
    app.MapControllers();
    // Seeding database async
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<RoomDbContextSeeder>();
        await seeder.InitialiseAsync();
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
