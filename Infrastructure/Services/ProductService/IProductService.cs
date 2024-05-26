using Domain.DTOs.ProductDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.ProductService;

public interface IProductService
{
    Task<PagedResponse<List<GetProductDto>>> GetProductsAsync(ProductFilter filter);
    Task<Response<GetProductDto>> GetProductByIdAsync(int productId);
    Task<Response<string>> CreateProductAsync(CreateProductDto createProduct);
    Task<Response<string>> UpdateProductAsync(UpdateProductDto updateProduct);
    Task<Response<bool>> DeleteProductAsync(int productId);
}