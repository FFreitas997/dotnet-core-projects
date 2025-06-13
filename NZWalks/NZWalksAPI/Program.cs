using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Mappings;
using NZWalksAPI.Middlewares;
using NZWalksAPI.Repositories.Implementations;
using NZWalksAPI.Repositories.Interface;
using NZWalksAPI.Services.Implementations;
using NZWalksAPI.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))
);

// Register services
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IWalkService, WalkService>();

// Register repositories
builder.Services.AddScoped<IRegionRepository, RegionRepositorySql>();
builder.Services.AddScoped<IWalkRepository, WalkRepository>();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Add Middleware for global exception handling
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

// Seed the database with initial data
await DbSeeder.SeedDifficultiesAsync(app.Services);

app.Run();