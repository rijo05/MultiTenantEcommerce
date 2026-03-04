using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Delete;

public record DeleteCustomerCommand(
    Guid Id) : ICommand<Unit>;