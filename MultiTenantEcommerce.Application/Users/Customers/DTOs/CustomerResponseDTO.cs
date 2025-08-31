using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Users.Customers.DTOs;
public class CustomerResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string CountryCode { get; init; }
    public AddressResponseDTO Address { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
