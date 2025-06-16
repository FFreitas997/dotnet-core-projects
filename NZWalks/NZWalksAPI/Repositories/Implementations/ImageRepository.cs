using NZWalksAPI.Data;
using NZWalksAPI.Models.Entities;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Repositories.Implementations;

public class ImageRepository(
    IWebHostEnvironment environment,
    IHttpContextAccessor accessor,
    ApplicationDbContext dbContext) : IImageRepository
{
    public async Task<Image> UploadAsync(Image entity)
    {
        var localPath = Path.Combine(environment.ContentRootPath, "Storage",
            $"{entity.FileName}{entity.FileExtension}");

        // Save the file to the local path
        using var stream = new FileStream(localPath, FileMode.Create);
        await entity.File.CopyToAsync(stream);

        // Update the entity properties
        var urlFilePath =
            $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}{accessor.HttpContext.Request.PathBase}/Storage/{entity.FileName}{entity.FileExtension}";
        entity.FilePath = urlFilePath;

        // save in the database
        await dbContext.Images.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }
}