using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechNest.Domain.Entites;

namespace TechNest.Infrastructure.Data.EntitesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);

        }
    }
}
