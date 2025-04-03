
using Mapster;
using TechNest.Domain.Entites;
using Microsoft.AspNetCore.Http;
using TechNest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TechNest.Application.Services;
using TechNest.Application.Interfaces;
using TechNest.Application.DTOs.Product;

namespace TechNest.Infrastructure.Repositores
{

    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbcontext dbContext;
        private readonly IImageManagmentService imageManagmentService;
        public ProductRepository(ApplicationDbcontext dbContext, IImageManagmentService imageManagmentService) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.imageManagmentService = imageManagmentService;
        }
        public async Task<bool> AddAsync(ProductCreateDto productCreateDto)
        {
            if (productCreateDto is null)
                return false;

            // 1. Create and save the product first
            var product = productCreateDto.Adapt<Product>();
            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            // 2. Skip photo processing if no photos
            if (productCreateDto.Photos == null || productCreateDto.Photos.Count == 0)
                return true;

            // 3. Filter out null or empty files BEFORE passing to the service
            var validFiles = new List<IFormFile>();
            foreach (var file in productCreateDto.Photos)
            {
                if (file != null && file.Length > 0)
                    validFiles.Add(file);
            }

            if (validFiles.Count == 0)
                return true;

            // 4. Process only the valid files
            var imagePaths = await imageManagmentService.AddPhotoAsync(validFiles, productCreateDto.Name);

            // 5. Create photo entities only for valid paths
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

            // 6. Save photos to database
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

                updateProductDto.Adapt<Product>();

                await dbContext.SaveChangesAsync();

                if (updateProductDto.Photos != null && updateProductDto.Photos.Count > 0)
                {
                    var existingPhotos = await dbContext.Photos
                        .Where(p => p.ProductId == updateProductDto.Id)
                        .ToListAsync();

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

