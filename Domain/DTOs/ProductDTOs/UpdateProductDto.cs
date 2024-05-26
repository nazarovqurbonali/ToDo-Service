using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.ProductDTOs;

public class UpdateProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public IFormFile? Photo { get; set; }
}