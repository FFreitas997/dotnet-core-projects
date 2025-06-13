using AutoMapper;
using NZWalksAPI.Exceptions;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Services.Implementations;

public class RegionService(IRegionRepository repository, ILogger<RegionService> logger, IMapper mapper) : IRegionService
{
    public async Task<List<RegionDto>> GetAllRegionsAsync()
    {
        var regions = await repository.GetAllAsync();

        return mapper.Map<List<RegionDto>>(regions);
    }

    public async Task<RegionDto> GetRegionByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Invalid region ID provided: {Guid}", id);
            throw new BadRequestException($"Invalid region with ID provided: {id}");
        }

        // Fetch the region by ID from the repository
        var region = await repository.GetByIdAsync(id);

        if (region == null)
        {
            logger.LogError("Region with ID {Guid} not found", id);
            throw new NotFoundException($"Region with ID {id} not found");
        }

        return mapper.Map<RegionDto>(region);
    }

    public async Task<RegionDto> CreateRegionAsync(CreateRegionRequestDto request)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogError("Region name is required");
            throw new ValidationException("Region name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            logger.LogError("Region code is required");
            throw new ValidationException("Region code is required");
        }

        if (!string.IsNullOrWhiteSpace(request.ImageUrl) &&
            !Uri.IsWellFormedUriString(request.ImageUrl, UriKind.Absolute))
        {
            logger.LogError("Invalid image URL provided: {ImageUrl}", request.ImageUrl);
            throw new ValidationException("Invalid image URL provided");
        }

        // Create new Region entity
        var newRegion = mapper.Map<Region>(request);

        // Add the new region to the repository
        newRegion = await repository.CreateAsync(newRegion);

        // Convert to DTO
        return mapper.Map<RegionDto>(newRegion);
    }

    public async Task<RegionDto> UpdateRegionAsync(Guid id, UpdateRegionRequest request)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Invalid region ID provided: {Guid}", id);
            throw new BadRequestException($"Invalid region with ID provided: {id}");
        }

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            logger.LogError("Region name is required");
            throw new ValidationException("Region name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            logger.LogError("Region code is required");
            throw new ValidationException("Region code is required");
        }

        if (!string.IsNullOrWhiteSpace(request.ImageUrl) &&
            !Uri.IsWellFormedUriString(request.ImageUrl, UriKind.Absolute))
        {
            logger.LogError("Invalid image URL provided: {ImageUrl}", request.ImageUrl);
            throw new ValidationException("Invalid image URL provided");
        }

        var region = await repository.GetByIdAsync(id);

        if (region == null)
        {
            logger.LogError("Region with ID {Guid} not found", id);
            throw new NotFoundException($"Region with ID {id} not found");
        }

        // Update the region properties
        region.Name = request.Name;
        region.Code = request.Code;
        region.ImageUrl = request.ImageUrl ?? region.ImageUrl; // Keep existing image URL if not provided
        region.UpdatedAt = DateTime.UtcNow; // Update the timestamp

        // Save changes to the repository
        region = await repository.UpdateAsync(region);

        return mapper.Map<RegionDto>(region);
    }

    public async Task DeleteRegionAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Invalid region ID provided: {Guid}", id);
            throw new BadRequestException($"Invalid region with ID provided: {id}");
        }

        var region = await repository.GetByIdAsync(id);

        if (region == null)
        {
            logger.LogError("Region with ID {Guid} not found", id);
            throw new NotFoundException($"Region with ID {id} not found");
        }

        // Delete the region from the repository
        await repository.DeleteAsync(region);
    }
}