
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TechNest.API.APIResponse;
using TechNest.Application.Interfaces;
using TechNest.Application.DTOs.Product;
using TechNest.Application.Services;
using TechNest.API.Model;
using TechNest.Domain.Entites;

namespace TechNest.API.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IImageManagmentService _imageManagementService;
        private readonly FileUploadSettings _fileUploadSettings;
        public ProductsController(IUnitOfWork unitOfWork, IImageManagmentService imageManagementService, FileUploadSettings fileUploadSettings, ILogger<ProductsController> logger) : base(unitOfWork)
        {
            _imageManagementService = imageManagementService;
            _fileUploadSettings = fileUploadSettings;
            _logger = logger;
        }

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
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(APIResponse<ProductCreateDto>), StatusCodes.Status200OK)]
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

                var product = productCreateDto.Adapt<Product>();
                var result = await _unitOfWork.ProductRepository.AddAsync(productCreateDto);

                if (result == null)
                    return BadRequest(new APIErrorResponse(400, "Failed to add product"));

                // Assume that product is successfully added, and create the response DTO
                var createdProduct = product.Adapt<GetProductDto>();

                var apiResponse = new APIResponse<GetProductDto>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = createdProduct,
                    StatusCode = 200,
                    Timestamp = DateTime.UtcNow
                };

                return Ok(apiResponse);
            }
            catch (Exception)
            {
                var errorResponse = new APIErrorResponse(500, "An error occurred while processing your request");
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            try
            {
                await _unitOfWork.ProductRepository.UpdateAsync(updateProductDto);
                return Ok(new APIResponse<string>("Product created successfully"));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }




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


    //    [HttpPost]
    //    [Consumes("multipart/form-data")]
    //    [ProducesResponseType(typeof(APIResponse<GetProductDto>), StatusCodes.Status200OK)]
    //    [ProducesResponseType(typeof(APIErrorResponse), StatusCodes.Status404NotFound)]
    //    public async Task<IActionResult> Add([FromForm] ProductCreateDto productCreateDto)
    //    {
    //        try
    //        {
    //            if (!ModelState.IsValid)
    //            {
    //                return BadRequest(new APIErrorResponse(400, "Invalid expense data"));
    //            }

    //            var product = productCreateDto.Adapt<Product>();
    //            await _unitOfWork.ProductRepository.AddAsync(product);

    //            if (productCreateDto.Photos?.Any() == true)
    //            {
    //                var (IsSuccess, ErrorMessage) = await UploadFiles(product.Id, productCreateDto.Photos);
    //                if (!IsSuccess)
    //                {
    //                    return BadRequest(new APIErrorResponse(400, ErrorMessage));
    //                }
    //            }

    //            await _unitOfWork.SaveChangesAsync();

    //            var createdProduct = product.Adapt<GetProductDto>();
    //            return CreatedAtAction(nameof(GetById), new { id = product.Id },
    //                new APIResponse<GetProductDto>(createdProduct, "Product created successfully"));
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error creating expense");
    //            return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
    //        }
    //    }
    //    private async Task<(bool IsSuccess, string ErrorMessage)> UploadFiles(Guid productId, IFormFileCollection files)
    //    {
    //        try
    //        {
    //            if (files.Count > _fileUploadSettings.MaxFileUploadLimit)
    //            {
    //                return (false, $"Maximum {_fileUploadSettings.MaxFileUploadLimit} files can be uploaded");
    //            }

    //            foreach (var file in files)
    //            {
    //                if (file.Length > _fileUploadSettings.MaxFileSize)
    //                {
    //                    return (false, $"File {file.FileName} exceeds maximum size of {_fileUploadSettings.MaxFileSize / 1024 / 1024}MB");
    //                }

    //                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
    //                if (!_fileUploadSettings.AllowedFileTypes.Contains(extension))
    //                {
    //                    return (false, $"File type {extension} is not allowed");
    //                }
    //            }

    //            var source = Path.Combine("Products", productId.ToString());
    //            var filePaths = await _imageManagementService.AddPhotoAsync(files, source);

    //            var productImages = filePaths.Select(filePath => new Photo
    //            {
    //                ProductId = productId,
    //                ImageName = filePath,
    //                UploadedAt = DateTime.UtcNow,

    //            }).ToList();



    //            await _unitOfWork.PhotoRepository.AddRangeAsync(productImages.ToArray());
    //            return (true, string.Empty);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error uploading files for expense: {ExpenseId}", productId);
    //            return (false, "Failed to upload files");
    //        }
    //    }
    }
}
