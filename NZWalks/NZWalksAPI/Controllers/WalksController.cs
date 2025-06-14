using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class WalksController(ILogger<RegionsController> logger, IWalkService service) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> Get(
        [FromQuery] string? name,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDirection,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10
    )
    {
        logger.LogInformation("Requesting all walks");

        var walks = await service.GetAllWalksAsync(name, sortBy, sortDirection, page, size);

        return Ok(walks);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        logger.LogInformation("Requesting walk with ID: {Guid}", id);

        var walk = await service.GetWalkByIdAsync(id);

        return Ok(walk);
    }

    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Post([FromBody] CreateWalkRequestDto request)
    {
        logger.LogInformation("Creating new walk: {string}", request.Name);

        var walk = await service.CreateWalkAsync(request);

        return CreatedAtAction(nameof(Get), new { id = walk.Id }, walk);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateWalkRequest request)
    {
        logger.LogInformation("Updating walk with ID: {Guid}", id);

        var walk = await service.UpdateWalkAsync(id, request);

        return Ok(walk);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting walk with ID: {Guid}", id);

        await service.DeleteWalkAsync(id);

        return NoContent();
    }
}