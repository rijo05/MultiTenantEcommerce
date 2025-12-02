using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.SetMainImage;
public record SetMainProductImageCommand(
    Guid ProductId,
    Guid ImageId) : ICommand<ProductResponseAdminDTO>;
