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
}