

using Microsoft.AspNetCore.Http;

namespace TechNest.Application.Services
{
    public interface IImageManagmentService
    {
        Task<List<string>> AddPhotoAsync(IFormFileCollection files, string source);
        void DeleteAsync(string source);
    }
}
