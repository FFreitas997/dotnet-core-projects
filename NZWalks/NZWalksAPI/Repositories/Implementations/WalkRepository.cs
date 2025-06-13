using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Repositories.Implementations;

public class WalkRepository(ApplicationDbContext dbContext) : IWalkRepository
{
    public async Task<Walk> CreateWalkAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        await dbContext.SaveChangesAsync();

        return walk;
    }

    public async Task<List<Walk>> GetAllAsync(string? name, string? sortBy, string? sortDirection, int page = 1,
        int size = 10)
    {
        var query = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

        // Apply filtering by name if provided
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(w => EF.Functions.Like(w.Name, $"%{name}%"));

        // Apply sorting if sortBy and sortDirection are provided
        switch (sortBy)
        {
            case "Name":
                if (!string.IsNullOrWhiteSpace(sortDirection) && sortDirection?.ToLower() == "desc")
                    query = query.OrderByDescending(w => w.Name);
                else
                    query = query.OrderBy(w => w.Name);
                break;

            case "LengthKm":
                if (!string.IsNullOrWhiteSpace(sortDirection) && sortDirection?.ToLower() == "desc")
                    query = query.OrderByDescending(w => w.LengthKm);
                else
                    query = query.OrderBy(w => w.LengthKm);
                break;

            default:
                query = query.OrderBy(w => w.Name);
                break;
        }

        // Apply pagination
        if (page < 1) page = 1; // Ensure page is at least 1
        if (size < 1) size = 10; // Ensure size is at least 1
        query = query.Skip((page - 1) * size).Take(size);

        return await query.ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await dbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Walk> UpdateAsync(Walk walk)
    {
        dbContext.Walks.Update(walk);
        await dbContext.SaveChangesAsync();

        return walk;
    }

    public async Task DeleteAsync(Walk walk)
    {
        dbContext.Walks.Remove(walk);
        await dbContext.SaveChangesAsync();
    }
}