using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechNest.Application.DTOs.Photos
{
    public record UpdatePhotDto : CreatePhotoDto
    {
        public Guid Id { get; set; }
    }
}
