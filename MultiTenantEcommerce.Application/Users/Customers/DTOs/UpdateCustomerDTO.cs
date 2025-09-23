using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Users.Customers.DTOs;
public record UpdateCustomerDTO(
    string? Name,
    string? Email,
    string? Password,
    string? PhoneNumber,
    string? CountryCode,
    CreateAddressDTO? Address);
