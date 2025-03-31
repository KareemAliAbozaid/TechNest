
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using TechNest.Domain.Entites;


namespace TechNest.Application.DTOs.Product
{
    public record ProductCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public string? Category { get; set; }
        public IFormFileCollection? Photos { get; set; }
    }
}
