namespace MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;

public record CustomerResponseDTO(
    Guid Id,
    string Name,
    string Email,
    List<AddressResponseDTO> Addresses,
    DateTime CreatedAt,
    DateTime UpdatedAt);