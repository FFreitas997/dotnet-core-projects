using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Services.Interface;

public interface IRegionService
{
    Task<List<Region>> GetAllRegionsAsync();
    Task<Region> GetRegionByIdAsync(Guid id);
    Task<Region> AddRegionAsync(Region region);
}