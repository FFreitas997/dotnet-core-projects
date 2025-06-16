using NZWalksAPI.Models.Entities;

namespace NZWalksAPI.Repositories.Interface;

public interface IImageRepository
{
    Task<Image> UploadAsync(Image entity);
}