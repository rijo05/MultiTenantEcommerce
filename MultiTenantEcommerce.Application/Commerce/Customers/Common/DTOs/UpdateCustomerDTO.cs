using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;

public record UpdateCustomerDTO(
    string? Name,
    string? Email,
    string? Password);