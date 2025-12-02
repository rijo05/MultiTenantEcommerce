using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetImage;
public record GetProductImageByIdQuery(
    Guid ProductId,
    Guid ImageId,
    bool IsAdmin) : IQuery<IProductImageDTO>;
