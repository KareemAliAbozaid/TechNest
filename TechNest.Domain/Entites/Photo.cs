
using System.ComponentModel.DataAnnotations.Schema;

namespace TechNest.Domain.Entites
{
   public class Photo : BaseEntity
    {
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public string? ImageName { get; set; }
        public DateTime UploadedAt { get; set; }

    }
}
