namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs;

public interface IProductDTO
{
    Guid Id { get; }
    string Name { get; }
    string? Description { get; }
    decimal Price { get; }
    public Guid CategoryId { get; }
}