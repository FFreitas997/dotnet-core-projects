using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Repositories.Implementations;

public class RegionRepositorySql(ApplicationDbContext dbContext) : IRegionRepository
{
    public async Task<List<Region>> GetAllAsync()
    {
        return await dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Region> CreateAsync(Region region)
    {
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();

        return region;
    }

    public async Task<Region> UpdateAsync(Region region)
    {
        dbContext.Regions.Update(region);
        await dbContext.SaveChangesAsync();

        return region;
    }

    public async Task DeleteAsync(Region region)
    {
        dbContext.Regions.Remove(region);
        await dbContext.SaveChangesAsync();
    }
}