

namespace TechNest.Application.DTOs.Photos
{
    public record CreatePhotoDto
    {
        public Guid ProductId { get; set; }
        public string? ImageName { get; set; }
        public DateTime? UploadedAt { get; set; }
    }
}

