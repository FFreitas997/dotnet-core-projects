using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Repositories.Interface;

public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync();
    Task<Region?> GetByIdAsync(Guid id);
    Task<Region> CreateAsync(Region region);
    Task<Region> UpdateAsync(Region region);
    Task DeleteAsync(Region region);
}