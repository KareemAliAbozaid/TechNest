

using Microsoft.AspNetCore.Http;

namespace TechNest.Application.Services
{
    public interface IImageManagmentService
    {
        Task<List<string>> AddPhotoAsync(IEnumerable<IFormFile> files, string source);
        void DeleteAsync(string source);
    }
}
