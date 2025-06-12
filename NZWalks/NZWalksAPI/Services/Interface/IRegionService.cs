using NZWalksAPI.Models.DTOs;

namespace NZWalksAPI.Services.Interface;

public interface IRegionService
{
    Task<List<RegionDto>> GetAllRegionsAsync();
    Task<RegionDto> GetRegionByIdAsync(Guid id);
    Task<RegionDto> CreateRegionAsync(CreateRegionRequestDto request);
    Task<RegionDto> UpdateRegionAsync(Guid id, UpdateRegionRequest request);
    Task DeleteRegionAsync(Guid id);
}