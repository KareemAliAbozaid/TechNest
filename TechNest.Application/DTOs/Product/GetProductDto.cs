﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechNest.Application.DTOs.Product
{
   public class GetProductDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }     
    }
}
