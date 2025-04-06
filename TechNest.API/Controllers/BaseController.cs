using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechNest.Application.Interfaces;

namespace TechNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public const string DefaultErrorMessage = "An unexpected error occurred while processing your request.";

        public BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
