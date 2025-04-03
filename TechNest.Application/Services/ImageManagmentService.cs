using Microsoft.AspNetCore.Http;
using TechNest.Application.Services;
using Microsoft.Extensions.FileProviders;

public class ImageManagmentService : IImageManagmentService
{
    private readonly IFileProvider fileProvider;

    public ImageManagmentService(IFileProvider fileProvider)
    {
        this.fileProvider = fileProvider;
    }

    public async Task<List<string>> AddPhotoAsync(IEnumerable<IFormFile> files, string source)
    {
        var filledImages = new List<string>();
        var imageDirctory = Path.Combine("wwwroot", "StorageFiles", source);
        if (Directory.Exists(imageDirctory) is not true)
        {
            Directory.CreateDirectory(imageDirctory);
        }
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var imageNames = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var imagePath = Path.Combine("StorageFiles", source, imageNames);

                var root = Path.Combine(imageDirctory, imageNames);

                using (var stream = new FileStream(root, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                filledImages.Add(imagePath);
            }
        }

        return filledImages;

    }


    public void DeleteAsync(string source)
    {
        var file = fileProvider.GetFileInfo(source);
        var root = file.PhysicalPath;
        File.Delete(root);
    }
}


