namespace Domain.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Photo { get; set; }
    
}