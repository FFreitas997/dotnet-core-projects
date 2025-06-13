using NZWalksAPI.Models.DTOs;

namespace NZWalksAPI.Services.Interface;

public interface IWalkService
{
    Task<WalkDto> CreateWalkAsync(CreateWalkRequestDto request);

    Task<List<WalkDto>> GetAllWalksAsync(string? name, string? sortBy, string? sortDirection, int page = 1,
        int size = 10);

    Task<WalkDto?> GetWalkByIdAsync(Guid id);
    Task<WalkDto> UpdateWalkAsync(Guid id, UpdateWalkRequest request);
    Task DeleteWalkAsync(Guid id);
}