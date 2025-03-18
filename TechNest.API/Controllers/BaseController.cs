using Microsoft.AspNetCore.Mvc;
using TechNest.Domain.Interface;

namespace TechNest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;

        public const string DefaultErrorMessage = "An unexpected error occurred while processing your request.";

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
