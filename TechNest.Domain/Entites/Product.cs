﻿
using System.ComponentModel.DataAnnotations.Schema;

namespace TechNest.Domain.Entites
{
   public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Photo>? Photos { get; set; }
    }
}
