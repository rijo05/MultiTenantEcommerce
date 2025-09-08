using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;

namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
public record UpdateCustomerCommand(
    Guid Id,
    string? Name,
    string? Email,
    string? Password,
    string? CountryCode,
    string? PhoneNumber,
    CreateAddressDTO? Address) : ICommand<CustomerResponseDTO>;
