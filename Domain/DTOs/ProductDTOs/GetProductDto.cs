namespace Domain.DTOs.ProductDTOs;

public class GetProductDto
{
    public int Id { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Photo { get; set; }
}