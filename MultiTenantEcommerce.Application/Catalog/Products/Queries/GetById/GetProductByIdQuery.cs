using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
public record GetProductByIdQuery(
    Guid ProductId,
    bool IsAdmin) : IQuery<IProductDTO>;