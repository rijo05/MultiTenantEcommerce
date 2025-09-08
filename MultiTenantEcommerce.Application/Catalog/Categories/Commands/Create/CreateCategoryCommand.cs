using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
public record CreateCategoryCommand(
    string Name,
    string? Description) : ICommand<CategoryResponseDTO>;
