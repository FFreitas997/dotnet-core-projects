using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Exceptions;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(ILogger<RegionsController> logger, IImageRepository repository) : ControllerBase
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    [HttpPost]
    [Route("Upload")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Upload([FromForm] UploadImageRequestDto request)
    {
        logger.LogInformation("Uploading image with name: {Name}", request.Name);

        // Validate file extension
        if (!_allowedExtensions.Contains(Path.GetExtension(request.File.FileName).ToLower()))
            throw new BadRequestException("Invalid File Extension");

        // Validate file size (max 10 MB)
        if (request.File.Length > 10 * 1024 * 1024)
            throw new BadRequestException("File size exceeds the maximum limit of 10 MB");

        // Create a new Image entity
        var image = new Image
        {
            File = request.File,
            FileName = request.Name,
            FileDescription = request.Description,
            FileExtension = Path.GetExtension(request.File.FileName).ToLower(),
            FileSizeInBytes = request.File.Length,
            FilePath = Path.Combine("uploads", request.Name + Path.GetExtension(request.File.FileName))
        };

        // Upload the image using the repository
        var uploadedImage = await repository.UploadAsync(image);

        if (uploadedImage == null)
            throw new BadRequestException("Image upload failed");

        return Ok(uploadedImage);
    }
}