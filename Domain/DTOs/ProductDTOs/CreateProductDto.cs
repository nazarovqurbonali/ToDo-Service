using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.ProductDTOs;

public class CreateProductDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public IFormFile? Photo { get; set; }
}