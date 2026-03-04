using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductResponseDTO>;