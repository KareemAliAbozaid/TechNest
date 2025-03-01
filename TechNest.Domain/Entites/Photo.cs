
using System.ComponentModel.DataAnnotations.Schema;

namespace TechNest.Domain.Entites
{
   public class Photo : BaseEntity
    {
        public string? ImageName { get; set; }
       
        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }
    }
}
