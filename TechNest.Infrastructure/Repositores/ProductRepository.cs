
using Mapster;
using TechNest.Domain.Entites;
using Microsoft.AspNetCore.Http;
using TechNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TechNest.Application.Services;
using TechNest.Application.Interfaces;
using TechNest.Application.DTOs.Product;
using AutoMapper;

namespace TechNest.Infrastructure.Repositores
{

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbcontext dbContext;
        private readonly IImageManagmentService imageManagmentService;
        private readonly IMapper mapper;
        public ProductRepository(ApplicationDbcontext dbContext, IImageManagmentService imageManagmentService, IMapper mapper) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.imageManagmentService = imageManagmentService;
            this.mapper = mapper;
        }
        public async Task<bool> AddAsync(ProductCreateDto productCreateDto)
        {
            if (productCreateDto is null)
                return false;

            // Map DTO to Product entity using AutoMapper
            var product = mapper.Map<Product>(productCreateDto);

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // Skip photo processing if no photos
            if (productCreateDto.Photos == null || productCreateDto.Photos.Count == 0)
                return true;

            // Filter out null or empty files BEFORE passing to the service
            var validFiles = new List<IFormFile>();
            foreach (var file in productCreateDto.Photos)
            {
                if (file != null && file.Length > 0)
                    validFiles.Add(file);
            }

            if (validFiles.Count == 0)
                return true;

            // Process only the valid files
            var imagePaths = await imageManagmentService.AddPhotoAsync(validFiles, productCreateDto.Name);

            // Create photo entities only for valid paths
            var photos = new List<Photo>();
            foreach (var path in imagePaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    photos.Add(new Photo
                    {
                        ImageName = path,
                        ProductId = product.Id
                    });
                }
            }

            // Save photos to the database
            if (photos.Count > 0)
            {
                await dbContext.Photos.AddRangeAsync(photos);
                await dbContext.SaveChangesAsync();
            }

            return true;
        }



        // update
        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto)
        {
            try
            {
                if (updateProductDto is null)
                    return false;

                // Map DTO to Product entity using AutoMapper
                var product = mapper.Map<Product>(updateProductDto);

                // Attach to the context to update it (this assumes the entity is already tracked)
                dbContext.Products.Update(product);
                await dbContext.SaveChangesAsync();

                // Handle photos if provided
                if (updateProductDto.Photos != null && updateProductDto.Photos.Count > 0)
                {
                    var existingPhotos = await dbContext.Photos
                        .Where(p => p.ProductId == updateProductDto.Id)
                        .ToListAsync();

                    // Delete existing photos
                    foreach (var photo in existingPhotos)
                    {
                        if (!string.IsNullOrEmpty(photo.ImageName))
                            imageManagmentService.DeleteAsync(photo.ImageName);
                    }

                    dbContext.Photos.RemoveRange(existingPhotos);
                    await dbContext.SaveChangesAsync();

                    var safeFolderName = string.Join("_",
                        (updateProductDto.Name ?? "product")
                        .Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

                    var imagePaths = await imageManagmentService.AddPhotoAsync(updateProductDto.Photos, safeFolderName);

                    if (imagePaths.Any())
                    {
                        var newPhotos = imagePaths.Select(path => new Photo
                        {
                            ImageName = path,
                            ProductId = updateProductDto.Id
                        }).ToList();

                        await dbContext.Photos.AddRangeAsync(newPhotos);
                        await dbContext.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

