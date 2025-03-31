

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace TechNest.Application.Services
{
    public class ImageManagmentService : IImageManagmentService
    {
        private readonly IFileProvider fileProvider;
        public ImageManagmentService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public async Task<List<string>> AddPhotoAsync(IFormFileCollection files, string source)
        {
            var savedImage = new List<string>();
            var imageDirectory = Path.Combine("wwwroot","Images",source);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
            foreach (var image in files)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var imagePath=Path.Combine("Images", source,imageName);
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
}
