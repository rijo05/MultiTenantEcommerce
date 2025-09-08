using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateCustomer;
public record CreateCustomerCommand(
    string Name,
    string Email,
    string Password,
    string CountryCode,
    string PhoneNumber,
    CreateAddressDTO Address) : ICommand<AuthCustomerResponseDTO>;
