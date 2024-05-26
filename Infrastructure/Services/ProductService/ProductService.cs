using System.Net;
using Domain.DTOs.ProductDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ProductService;

public class ProductService
    (IFileService fileService, ILogger<ProductService> logger, DataContext context) : IProductService
{
    #region GetProductsAsync

    public async Task<PagedResponse<List<GetProductDto>>> GetProductsAsync(ProductFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetProductsAsync} in time:{DateTime} ", "GetProductsAsync",
                DateTimeOffset.UtcNow);

            var products = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
                products = products.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            var response = await products.Select(x => new GetProductDto()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
                Photo = x.Photo,
                Price = x.Price,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await products.CountAsync();

            logger.LogInformation("Finished method {GetProductsAsync} in time:{DateTime} ", "GetProductsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetProductDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetProductDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetProductByIdAsync

    public async Task<Response<GetProductDto>> GetProductByIdAsync(int productId)
    {
        try
        {
            logger.LogInformation("Starting method {GetProductByIdAsync} in time:{DateTime} ", "GetProductByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Products.Where(x => x.Id == productId).Select(x => new GetProductDto()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id,
                Photo = x.Photo,
                Price = x.Price,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found product with id={Id},time={DateTimeNow}", productId, DateTime.UtcNow);
                return new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not found");
            }

            logger.LogInformation("Finished method {GetProductByIdAsync} in time:{DateTime} ", "GetProductByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetProductDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetProductDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateProductAsync

    public async Task<Response<string>> CreateProductAsync(CreateProductDto createProduct)
    {
        try
        {
            logger.LogInformation("Starting method {CreateProductAsync} in time:{DateTime} ", "CreateProductAsync",
                DateTimeOffset.UtcNow);
            var newProduct = new Product()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Description = createProduct.Description,
                Name = createProduct.Name,
                Price = createProduct.Price,
                Photo = createProduct.Photo == null ? "null" : await fileService.CreateFile(createProduct.Photo)
            };
            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateProductAsync} in time:{DateTime} ", "CreateProductAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new product by id:{newProduct.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateProductAsync

    public async Task<Response<string>> UpdateProductAsync(UpdateProductDto updateProduct)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateProductAsync} in time:{DateTime} ", "UpdateProductAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Products.FirstOrDefaultAsync(x => x.Id == updateProduct.Id);
            if (existing is null)
            {
                logger.LogWarning("Product not found by id:{Id},time:{DateTimeNow} ", updateProduct.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Product not found");
            }

            if (updateProduct.Photo != null)
            {
                if (existing.Photo != null) fileService.DeleteFile(existing.Photo);
                existing.Photo = await fileService.CreateFile(updateProduct.Photo);
            }

            existing.Description = updateProduct.Description;
            existing.Name = updateProduct.Name!;
            existing.Price = existing.Price;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateProductAsync} in time:{DateTime} ", "UpdateProductAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated product by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteProductAsync

    public async Task<Response<bool>> DeleteProductAsync(int productId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteProductAsync} in time:{DateTime} ", "DeleteProductAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Product not found by id:{productId}");
            if (existing.Photo != null)
                fileService.DeleteFile(existing.Photo);
            context.Products.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteProductAsync} in time:{DateTime} ", "DeleteProductAsync",
                DateTimeOffset.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}