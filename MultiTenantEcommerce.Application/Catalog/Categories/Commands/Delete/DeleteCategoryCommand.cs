using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Delete;
public record DeleteCategoryCommand(
    Guid id) : ICommand<Unit>;
