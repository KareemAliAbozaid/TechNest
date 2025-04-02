
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TechNest.API.APIResponse;
using TechNest.Application.DTOs.Category;
using TechNest.Application.DTOs.Photos;
using TechNest.Application.DTOs.Product;
using TechNest.Application.Interfaces;
using TechNest.Domain.Entites;

namespace TechNest.API.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                if (products == null)
                    return NotFound(new APIErrorResponse(404, "No products found"));

                var mappedProducts = products.Adapt<List<GetProductDto>>();
                return Ok(new APIResponse<List<GetProductDto>>(mappedProducts, "Products retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new APIErrorResponse(404, $"Product with ID {id} not found"));

                var mappedProduct = product.Adapt<GetProductDto>();
                return Ok(mappedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductCreateDto productCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new APIErrorResponse(400, "Invalid product data"));

                var result = await _unitOfWork.ProductRepository.AddAsync(productCreateDto);
                if (!result)
                    return BadRequest(new APIErrorResponse(400, "Failed to add product"));

                return Ok(new { Message = "Product added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, "An error occurred while processing your request"));
            }
        }
    }
}
