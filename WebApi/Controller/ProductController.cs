using Domain.DTOs.ProductDTOs;
using Domain.Filters;
using Infrastructure.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet("products")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
    {
        var res1 = await productService.GetProductsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{productId:int}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetProductById(int productId)
    {
        var res1 = await productService.GetProductByIdAsync(productId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto createProduct)
    {
        var result = await productService.CreateProductAsync(createProduct);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto updateProduct)
    {
        var result = await productService.UpdateProductAsync(updateProduct);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{productId:int}")]
    public async Task<IActionResult> ChangePassword(int productId)
    {
        var result = await productService.DeleteProductAsync(productId);
        return StatusCode(result.StatusCode, result);
    }
}