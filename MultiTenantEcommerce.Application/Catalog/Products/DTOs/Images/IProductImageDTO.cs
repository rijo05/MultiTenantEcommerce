namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public interface IProductImageDTO
{
    Guid Id { get; }
    string PresignUrl { get; }
    bool IsMain { get; }
    string ContentType { get; }
}
