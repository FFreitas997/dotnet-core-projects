using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Services.Interface;

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController(ILogger<RegionsController> logger, IRegionService regionService) : ControllerBase
{
    [HttpGet]
    public IActionResult AllRegionsRequest()
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
    public IActionResult CreateRegion([FromBody] CreateRegionRequestDto request)
    {
        logger.LogInformation("Creating new region: {string}", request.Name);

        var region = regionService.CreateRegionAsync(request).Result;

        return CreatedAtAction(nameof(GetRegionById), new { id = region.Id }, region);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public IActionResult UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequest request)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Invalid region ID provided: {Guid}", id);
            return BadRequest("Invalid region ID.");
        }

        if (request == null)
        {
            logger.LogError("Update request is null for region ID: {Guid}", id);
            return BadRequest("Update request cannot be null.");
        }

        logger.LogInformation("Updating region with ID: {Guid}", id);

        var region = regionService.UpdateRegionAsync(id, request).Result;

        return Ok(region);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public IActionResult DeleteRegion([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            logger.LogError("Invalid region ID provided: {Guid}", id);
            return BadRequest("Invalid region ID.");
        }

        logger.LogInformation("Deleting region with ID: {Guid}", id);

        regionService.DeleteRegionAsync(id).Wait();

        return NoContent();
    }
}