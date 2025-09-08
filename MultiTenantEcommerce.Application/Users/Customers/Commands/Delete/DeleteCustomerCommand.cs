using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;


namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Delete;
public record DeleteCustomerCommand(
    Guid Id) : ICommand<Unit>;
