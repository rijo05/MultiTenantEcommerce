using MediatR;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Update;

public record UpdateCustomerCommand(
    Guid Id,
    string? Name,
    string? Email,
    string? Password) : ICommand<Unit>;