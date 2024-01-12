using AutoMapper;
using BookingApi.Data;
using BookingApi.DTO.Validator;
using BookingApi.Helpers;
using BookingApi.Interfaces;
using BookingApi.Repositories;
using BookingApi.Services;
using BookingApi.Services.Interfaces;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<UserRequestDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<RoleRequestDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<RoleDetailDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<AccountRequestDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<BookingTourRequestDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<TourRequestDTOValidator>();
        config.RegisterValidatorsFromAssemblyContaining<ScheduleRequestDTOValidator>();
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set lowercase for controller (routing)
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add Cors
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Add Connection
var connectionString = builder.Configuration.GetConnectionString("default");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add repository, interface
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Add service, interface
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add auto mapper
var automapper = new MapperConfiguration(item => item.AddProfile(new MappingProfiles()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
