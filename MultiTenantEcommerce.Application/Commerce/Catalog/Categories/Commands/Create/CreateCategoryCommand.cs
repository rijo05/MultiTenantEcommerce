using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Create;

public record CreateCategoryCommand(
    string Name,
    string? Description) : ICommand<CategoryResponseAdminDTO>;