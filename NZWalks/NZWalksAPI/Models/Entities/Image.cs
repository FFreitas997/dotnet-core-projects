using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalksAPI.Models.Entities;

public class Image
{
    public Guid Id { get; set; }

    [NotMapped] public required IFormFile File { get; set; }

    public required string FileName { get; set; }

    public string? FileDescription { get; set; }

    public required string FileExtension { get; set; }

    public long FileSizeInBytes { get; set; }

    public required string FilePath { get; set; }
}