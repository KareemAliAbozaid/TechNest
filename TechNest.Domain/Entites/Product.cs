
using System.ComponentModel.DataAnnotations.Schema;

namespace TechNest.Domain.Entites
{
   public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Photo>? Photos { get; set; } = new List<Photo>();
    }
}
