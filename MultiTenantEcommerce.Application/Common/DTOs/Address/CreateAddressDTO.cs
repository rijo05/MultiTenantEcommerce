namespace MultiTenantEcommerce.Application.Common.DTOs.Address;

public record CreateAddressDTO(
    string Street,
    string City,
    string PostalCode,
    string Country,
    string HouseNumber);
