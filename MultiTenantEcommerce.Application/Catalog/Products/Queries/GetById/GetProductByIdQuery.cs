using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
public record GetProductByIdQuery(
    Guid ProductId) : IQuery<ProductResponseDTO>;