using AutoMapper;
using NZWalksAPI.Exceptions;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Services.Implementations;

public class WalkService(ILogger<WalkService> logger, IMapper mapper, IWalkRepository repository) : IWalkService
{
    public async Task<WalkDto> CreateWalkAsync(CreateWalkRequestDto request)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogError("Walk name is required");
            throw new ValidationException("Walk name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            logger.LogError("Walk description is required");
            throw new ValidationException("Walk description is required");
        }

        if (request.LengthKm <= 0)
        {
            logger.LogError("Walk length must be greater than zero");
            throw new ValidationException("Walk length must be greater than zero");
        }

        if (request.DifficultyId == Guid.Empty)
        {
            logger.LogError("Difficulty ID is required for the walk");
            throw new ValidationException("Difficulty ID is required for the walk");
        }

        if (request.RegionId == Guid.Empty)
        {
            logger.LogError("Region ID is required for the walk");
            throw new ValidationException("Region ID is required for the walk");
        }

        // Map the request to the Walk entity
        var walk = mapper.Map<Walk>(request);

        // Save the walk to the repository
        await repository.CreateWalkAsync(walk);

        // Map the saved walk back to a DTO
        return mapper.Map<WalkDto>(walk);
    }

    public async Task<List<WalkDto>> GetAllWalksAsync(string? name, string? sortBy, string? sortDirection, int page = 1,
        int size = 10)
    {
        var walks = await repository.GetAllAsync(name, sortBy, sortDirection, page, size);

        return mapper.Map<List<WalkDto>>(walks);
    }

    public async Task<WalkDto?> GetWalkByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Walk ID cannot be empty");
            throw new ValidationException("Walk ID cannot be empty");
        }

        var walk = await repository.GetByIdAsync(id);

        if (walk == null)
        {
            logger.LogError("Walk with ID {Guid} not found", id);
            throw new NotFoundException($"Walk with ID {id} not found");
        }

        return mapper.Map<WalkDto>(walk);
    }

    public async Task<WalkDto> UpdateWalkAsync(Guid id, UpdateWalkRequest request)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Walk ID cannot be empty");
            throw new ValidationException("Walk ID cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogError("Walk name is required");
            throw new ValidationException("Walk name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            logger.LogError("Walk description is required");
            throw new ValidationException("Walk description is required");
        }

        if (request.LengthKm <= 0)
        {
            logger.LogError("Walk length must be greater than zero");
            throw new ValidationException("Walk length must be greater than zero");
        }

        if (request.DifficultyId == Guid.Empty)
        {
            logger.LogError("Difficulty ID is required for the walk");
            throw new ValidationException("Difficulty ID is required for the walk");
        }

        if (request.RegionId == Guid.Empty)
        {
            logger.LogError("Region ID is required for the walk");
            throw new ValidationException("Region ID is required for the walk");
        }

        var walk = await repository.GetByIdAsync(id);

        if (walk == null)
        {
            logger.LogError("Walk with ID {Guid} not found", id);
            throw new NotFoundException($"Walk with ID {id} not found");
        }

        walk.Name = request.Name;
        walk.Description = request.Description;
        walk.LengthKm = request.LengthKm;
        walk.ImageUrl = request.ImageUrl;
        walk.DifficultyId = request.DifficultyId;
        walk.RegionId = request.RegionId;

        walk = await repository.UpdateAsync(walk);

        return mapper.Map<WalkDto>(walk);
    }

    public async Task DeleteWalkAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Walk ID cannot be empty");
            throw new ValidationException("Walk ID cannot be empty");
        }

        var walk = await repository.GetByIdAsync(id);

        if (walk == null)
        {
            logger.LogError("Walk with ID {Guid} not found", id);
            throw new NotFoundException($"Walk with ID {id} not found");
        }

        await repository.DeleteAsync(walk);
    }
}