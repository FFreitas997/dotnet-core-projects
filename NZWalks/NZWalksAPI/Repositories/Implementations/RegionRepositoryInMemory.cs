using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Repositories.Implementations;

public class RegionRepositoryInMemory : IRegionRepository
{
    public async Task<List<Region>> GetAllAsync()
    {
        // Simulating asynchronous operation with Task.FromResult
        return await Task.FromResult(new List<Region>
        {
            new() { Id = Guid.NewGuid(), Name = "North Island", Code = "NI", ImageUrl = "https://www.example.com" },
            new() { Id = Guid.NewGuid(), Name = "South Island", Code = "SI", ImageUrl = "https://www.example.com" }
        });
    }

    public async Task<Region> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Region> CreateAsync(Region region)
    {
        throw new NotImplementedException();
    }

    public async Task<Region> UpdateAsync(Region region)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Region region)
    {
        throw new NotImplementedException();
    }
}