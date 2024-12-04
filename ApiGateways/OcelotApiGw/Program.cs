using Microsoft.OpenApi.Models;
using Serilog;
using OcelotApiGw.Extensions;
using Ocelot.Middleware;
var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName} up");
try
{
	//Add App Configuration 
	builder.AddAppConfigurations();

	// Add services to the container.

	builder.Services.AddControllers();
	// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
	//Add Ocelot
	builder.Services.ConfigureOcelot(builder.Configuration,builder.Environment.EnvironmentName);
	//Add Cors
	builder.Services.ConfigureCors(builder.Configuration);
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(
		options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem - Ocelot API Gateway", Version = "v1" });
		}
	);
	// Add Redis Distributed Caching
	builder.Services.AddStackExchangeRedisCache(options =>
	{
		options.Configuration = "redis-container:6379";
		options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
		{
			AbortOnConnectFail = true,
			EndPoints = { "redis-container:6379" },
			DefaultDatabase = 4 // Use database 4
		};
	});
	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
	{
		app.UseSwagger();
		app.UseSwaggerForOcelotUI();
	}

	app.UseCors("CorsPolicy");
	app.UseWebSockets();
	app.UseRouting();

	app.UseOcelot().Wait();

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
