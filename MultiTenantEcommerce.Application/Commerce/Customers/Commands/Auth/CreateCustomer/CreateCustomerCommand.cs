using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Unit>;