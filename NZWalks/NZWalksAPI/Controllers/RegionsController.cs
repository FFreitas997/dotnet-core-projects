using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(ILogger<RegionsController> logger, IRegionService regionService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllRegions()
    {
        logger.LogInformation("Fetching all regions");

        var regions = regionService.GetAllRegionsAsync().Result;

        return Ok(regions);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetRegionById([FromRoute] Guid id)
    {
        logger.LogInformation("Fetching region with ID: {Guid}", id);
        
        var region = regionService.GetRegionByIdAsync(id).Result;

        return Ok(region);
    }

    [HttpPost]
    public IActionResult AddRegion([FromBody] Region region)
    {
        logger.LogInformation("Adding new region: {RegionName}", region.Name);
        
        var addedRegion = regionService.AddRegionAsync(region).Result;
        
        return CreatedAtAction(nameof(GetRegionById), new { id = addedRegion.Id }, addedRegion);
    }
}