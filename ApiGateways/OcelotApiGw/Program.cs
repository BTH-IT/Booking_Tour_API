<<<<<<< HEAD
=======
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
using Microsoft.OpenApi.Models;
using Serilog;
using OcelotApiGw.Extensions;
using Ocelot.Middleware;
<<<<<<< HEAD
=======
using System.Text;
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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
<<<<<<< HEAD
=======

	var app = builder.Build();
>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker"))
	{
		app.UseSwagger();
		app.UseSwaggerForOcelotUI();
	}
	app.UseCors("CorsPolicy");

<<<<<<< HEAD
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

=======
	app.UseRouting();

	app.UseAuthentication();

	app.UseOcelot().Wait();

	app.UseAuthorization();

>>>>>>> 8ea5293bc147863998b5331d4fd7eb2f4226a11a
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
