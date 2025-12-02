namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponseDTO : IProductImageDTO
{
    public Guid Id { get; init; }
    public string PresignUrl { get; init; }
    public bool IsMain { get; init; }
    public string ContentType { get; init; }
}
