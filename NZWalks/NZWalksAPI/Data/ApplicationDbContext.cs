using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Walk> Walks { get; set; }

    public DbSet<Difficulty> Difficulties { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Image> Images { get; set; }
}