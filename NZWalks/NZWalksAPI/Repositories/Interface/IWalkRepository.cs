using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Repositories.Interface;

public interface IWalkRepository
{
    Task<Walk> CreateWalkAsync(Walk walk);
    Task<List<Walk>> GetAllAsync(string? name, string? sortBy, string? sortDirection, int page = 1, int size = 10);
    Task<Walk?> GetByIdAsync(Guid id);
    Task<Walk> UpdateAsync(Walk walk);
    Task DeleteAsync(Walk walk);
}