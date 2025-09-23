using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
public record UpdateCategoryCommand(
    Guid CategoryId,
    string? Name,
    string? Description,
    bool? IsActive) : ICommand<CategoryResponseAdminDTO>;