using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Services.Implementations;

public class RegionService(ApplicationDbContext dbContext) : IRegionService
{
    public async Task<List<Region>> GetAllRegionsAsync()
    {
        var regions = await dbContext.Regions.ToListAsync();

        if (regions == null || !regions.Any())
        {
            throw new Exception("No regions found");
        }

        return regions;
    }

    public async Task<Region> GetRegionByIdAsync(Guid id)
    {
        var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        
        if (region == null)
        {
            throw new Exception($"Region with ID {id} not found");
        }

        return region;
    }

    public async Task<Region> AddRegionAsync(Region region)
    {
        if (region == null)
        {
            throw new ArgumentNullException(nameof(region), "Region cannot be null");
        }
        await dbContext.Regions.AddAsync(region);
        await dbContext.SaveChangesAsync();
        return region;
    }
}