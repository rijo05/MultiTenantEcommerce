namespace MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;

public record AddressResponseDTO(
    Guid Id,
    string Street,
    string City,
    string PostalCode,
    string Country,
    string HouseNumber,
    bool IsDefaultAddress);