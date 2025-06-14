using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Data;

public static class DbSeeder
{
    public static async Task SeedDifficultiesAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Migrate the database to ensure it is up to date
        await dbContext.Database.MigrateAsync();

        if (!await dbContext.Difficulties.AnyAsync())
        {
            var difficulties = new List<Difficulty>
            {
                new() { Name = "Easy" },
                new() { Name = "Medium" },
                new() { Name = "Hard" }
            };

            await dbContext.Difficulties.AddRangeAsync(difficulties);
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task SeedRegionsAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // Migrate the database to ensure it is up to date
        await dbContext.Database.MigrateAsync();
        if (!await dbContext.Regions.AnyAsync())
        {
            var regions = new List<Region>
            {
                new() { Name = "North Island", Code = "NI" },
                new() { Name = "South Island", Code = "SI" }
            };
            await dbContext.Regions.AddRangeAsync(regions);
            await dbContext.SaveChangesAsync();
        }
    }

    public static async Task SeedUserRolesAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var authDbContext = scope.ServiceProvider.GetRequiredService<ApplicationAuthDbContext>();

        const string readerRoleId = "96f9f97f-0811-41ee-a087-b8bc75256abc";
        const string writerRoleId = "880ee893-aa07-4d41-a550-d4ba8ec84498";

        // Migrate the database to ensure it is up to date
        await authDbContext.Database.MigrateAsync();

        if (!await authDbContext.Roles.AnyAsync())
        {
            var roles = new List<IdentityRole>
            {
                new()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER",
                    ConcurrencyStamp = readerRoleId
                },
                new()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER",
                    ConcurrencyStamp = writerRoleId
                }
            };

            await authDbContext.Roles.AddRangeAsync(roles);
            await authDbContext.SaveChangesAsync();
        }
    }
}