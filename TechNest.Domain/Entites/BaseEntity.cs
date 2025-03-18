
namespace TechNest.Domain.Entites
{
    public class BaseEntity
    {
        public Guid Id { get; set; } 
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

    }
}
