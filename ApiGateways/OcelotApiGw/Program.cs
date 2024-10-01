using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using OcelotApiGw.Extensions;
using Ocelot.Middleware;
using System.Text;
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
	builder.Services.ConfigureOcelot(builder.Configuration);
	//Add Cors
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
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(
		options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingSystem - Identity API", Version = "v1" });
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

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}
	app.UseCors("CorsPolicy");

	//app.UseHttpsRedirection();

	app.UseRouting();

	app.UseAuthentication();

	app.UseAuthorization();

	app.MapControllers();

	app.UseOcelot().Wait();

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
