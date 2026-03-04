using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Delete;

public record DeleteCategoryCommand(
    Guid Id) : ICommand<Unit>;