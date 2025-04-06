using AutoMapper;
using TechNest.Application.DTOs.Category;
using TechNest.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;
using TechNest.API.APIResponse;
using TechNest.Domain.Entites;

namespace TechNest.API.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                if (categories == null)
                    return NotFound(new APIErrorResponse(404, "No categories found"));

                var mappedCategories = _mapper.Map<List<CategoryGetDto>>(categories);
                return Ok(new APIResponse<List<CategoryGetDto>>(mappedCategories, "Categories retrieved successfully"));
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
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new APIErrorResponse(404, $"Category with ID {id} not found"));

                var mappedCategory = _mapper.Map<CategoryGetDto>(category);
                return Ok(mappedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new APIErrorResponse(400, "Invalid Category data"));

                var category = _mapper.Map<Category>(categoryDto);

                await _unitOfWork.CategoryRepository.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                var createdCategory = _mapper.Map<CategoryGetDto>(category);

                return CreatedAtAction(nameof(GetById), new { id = category.Id },
                    new APIResponse<CategoryGetDto>(createdCategory, "Category created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (existingCategory is null)
                    return NotFound(new APIErrorResponse(404, $"Category with ID {id} not found"));

                _mapper.Map(categoryUpdateDto, existingCategory);  // Mapping Update
                await _unitOfWork.CategoryRepository.UpdateAsync(existingCategory);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { message = "Item Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category is null)
                    return NotFound(new APIErrorResponse(404, $"Category with ID {id} not found"));

                category.IsDeleted = true;
                category.DeletedAt = DateTime.Now;

                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new { message = "Item Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIErrorResponse(500, DefaultErrorMessage));
            }
        }
    }
}
