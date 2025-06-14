using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(ILogger<RegionsController> logger, IRegionService regionService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> AllRegionsRequest()
    {
        logger.LogInformation("Requesting all regions");

        var regions = await regionService.GetAllRegionsAsync();

        return Ok(regions);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
    {
        logger.LogInformation("Requesting region with ID: {Guid}", id);

        var region = await regionService.GetRegionByIdAsync(id);

        return Ok(region);
    }

    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionRequestDto request)
    {
        logger.LogInformation("Creating new region: {string}", request.Name);

        var region = await regionService.CreateRegionAsync(request);

        return CreatedAtAction(nameof(GetRegionById), new { id = region.Id }, region);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequest request)
    {
        logger.LogInformation("Updating region with ID: {Guid}", id);

        var region = await regionService.UpdateRegionAsync(id, request);

        return Ok(region);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting region with ID: {Guid}", id);

        await regionService.DeleteRegionAsync(id);

        return NoContent();
    }
}