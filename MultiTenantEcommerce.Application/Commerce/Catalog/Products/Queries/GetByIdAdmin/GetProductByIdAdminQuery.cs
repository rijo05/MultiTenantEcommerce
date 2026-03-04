using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetByIdAdmin;

public record GetProductByIdAdminQuery(Guid ProductId) : IQuery<ProductResponseAdminDTO>;
