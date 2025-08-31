using MediatR;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    int? Quantity,
    int? MinimumQuantity) : ICommand<ProductResponseDTO>;
