using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Services.Implementations;

public class RegionService(ApplicationDbContext dbContext) : IRegionService
{
    public async Task<List<RegionDto>> GetAllRegionsAsync()
    {
        var regions = await dbContext.Regions.ToListAsync();

        if (regions == null)
            throw new Exception("Something went wrong with the list of regions");

        // Convert to DTOs
        var response = regions
            .Select(region => new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                ImageUrl = region.ImageUrl
            })
            .ToList();

        return response;
    }

    public async Task<RegionDto> GetRegionByIdAsync(Guid id)
    {
        var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region == null)
            throw new Exception($"Region with ID {id} not found");

        // Convert to DTO
        var response = new RegionDto
        {
            Id = region.Id,
            Name = region.Name,
            Code = region.Code,
            ImageUrl = region.ImageUrl
        };

        return response;
    }

    public async Task<RegionDto> CreateRegionAsync(CreateRegionRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Region cannot be null");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Region name is required", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Code))
            throw new ArgumentException("Region code is required", nameof(request.Code));

        // Create new Region entity
        var newRegion = new Region
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Code = request.Code,
            ImageUrl = request.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await dbContext.Regions.AddAsync(newRegion);
        await dbContext.SaveChangesAsync();

        // Convert to DTO
        var response = new RegionDto
        {
            Id = newRegion.Id,
            Name = newRegion.Name,
            Code = newRegion.Code,
            ImageUrl = newRegion.ImageUrl
        };

        return response;
    }

    public async Task<RegionDto> UpdateRegionAsync(Guid id, UpdateRegionRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Region cannot be null");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Region name is required", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Code))
            throw new ArgumentException("Region code is required", nameof(request.Code));

        var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region == null)
            throw new Exception($"Region with ID {id} not found");

        // Update properties
        region.Name = request.Name;
        region.Code = request.Code;
        region.ImageUrl = request.ImageUrl;
        region.UpdatedAt = DateTime.UtcNow;

        dbContext.Regions.Update(region);
        await dbContext.SaveChangesAsync();

        // Convert to DTO
        var response = new RegionDto
        {
            Id = region.Id,
            Name = region.Name,
            Code = region.Code,
            ImageUrl = region.ImageUrl
        };

        return response;
    }

    public async Task DeleteRegionAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid region ID", nameof(id));

        var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region == null)
            throw new Exception($"Region with ID {id} not found");

        dbContext.Regions.Remove(region);
        await dbContext.SaveChangesAsync();
    }
}