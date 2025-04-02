using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using TechNest.Application.DTOs.Product;
using TechNest.Application.Interfaces;
using TechNest.Application.Services;
using TechNest.Domain.Entites;

public class ImageManagmentService : IImageManagmentService
{
    private readonly IFileProvider fileProvider;

    public ImageManagmentService(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider;
    }

    public async Task<List<string>> AddPhotoAsync(IEnumerable<IFormFile> files, string source)
    {
        var savedImage = new List<string>();

        if (files == null || !files.Any())
        {
            return savedImage;
        }

        var imageDirectory = Path.Combine("wwwroot", "Images", source);
        if (!Directory.Exists(imageDirectory))
        {
            Directory.CreateDirectory(imageDirectory);
        }

        foreach (var image in files)
        {
            // Skip null or empty files
            if (image == null || image.Length == 0)
                continue;

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var imagePath = Path.Combine("Images", source, imageName);
            var root = Path.Combine(imageDirectory, imageName);

            using (var stream = new FileStream(root, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            savedImage.Add(imagePath);
        }

        return savedImage;
    }

    public void DeleteAsync(string source)
    {
        var file = fileProvider.GetFileInfo(source);
        var root = file.PhysicalPath;
        File.Delete(root);
    }
}

