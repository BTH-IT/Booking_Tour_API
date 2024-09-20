using FluentValidation.AspNetCore;
using Room.API;
using Room.API.Extensions;
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
    builder.Services.AddControllers().AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
		options.JsonSerializerOptions.MaxDepth = 64; // Tùy chọn: Tăng độ sâu tối đa nếu cần
	}); ;
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    // Add Auto Mapper
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
	// Add Fluent Validator 
	builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<HotelRequestDTOValidator>());
	builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<RoomRequestDTOValidator>());
	//builder.Services.AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<AccountRequestDTOValidator>());
	//builder.Services.Configure<ApiBehaviorOptions>(options =>
	//{
	//    options.SuppressModelStateInvalidFilter = true;
	//});
	// Add DbContext
	builder.Services.ConfigureIdentityDbContext();
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

    //app.UseHttpsRedirection();

    app.UseAuthorization();

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
