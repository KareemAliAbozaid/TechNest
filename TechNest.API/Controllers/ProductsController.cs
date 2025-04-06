using Microsoft.AspNetCore.Mvc;
using TechNest.API.APIResponse;
using TechNest.Application.Interfaces;
using TechNest.Application.DTOs.Product;
using AutoMapper;
using TechNest.API.Model;
using TechNest.Application.Services;
using TechNest.Domain.Entites;

namespace TechNest.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IImageManagmentService _imageManagementService;
        private readonly FileUploadSettings _fileUploadSettings;
        private readonly IMapper _mapper;

        public ProductsController(
            IUnitOfWork unitOfWork,
            IImageManagmentService imageManagementService,
            FileUploadSettings fileUploadSettings,
            ILogger<ProductsController> logger,
            IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _imageManagementService = imageManagementService;
            _fileUploadSettings = fileUploadSettings;
            _logger = logger;
            _mapper = mapper;
        }

        // Get All Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                if (products == null)
                    return NotFound(new APIErrorResponse(404, "No products found"));

                var mappedProducts = _mapper.Map<List<GetProductDto>>(products);
                return Ok(new APIResponse<List<GetProductDto>>(mappedProducts, "Products retrieved successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        // Get Product By Id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                    return NotFound(new APIErrorResponse(404, $"Product with ID {id} not found"));

                var mappedProduct = _mapper.Map<GetProductDto>(product);
                return Ok(mappedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        // Add Product
        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIResponse<GetProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add([FromForm] ProductCreateDto productCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new APIErrorResponse(400, "Invalid product data"));
                }

                var result = await _unitOfWork.ProductRepository.AddAsync(productCreateDto);

   




                return Ok();
            }
            catch (Exception)
            {
                var errorResponse = new APIErrorResponse(500, "An error occurred while processing your request");
                return StatusCode(500, errorResponse);
            }
        }

        // Update Product
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            try
            {
          
                await _unitOfWork.ProductRepository.UpdateAsync(updateProductDto);
                return Ok(new APIResponse<string>("Product updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new APIErrorResponse(400, ex.Message));
            }
        }

        // Delete Product
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new APIErrorResponse(404, $"Product with ID {id} not found"));
                }

                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                return Ok(new APIResponse<string>(null, $"Product successfully deleted"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID: {ProductId}", id);
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }
    }
}
