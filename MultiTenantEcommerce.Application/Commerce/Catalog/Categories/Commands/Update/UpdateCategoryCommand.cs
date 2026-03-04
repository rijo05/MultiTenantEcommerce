using MediatR;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Update;

public record UpdateCategoryCommand(
    Guid CategoryId,
    string? Name,
    string? Description,
    bool? IsActive) : ICommand<Unit>;