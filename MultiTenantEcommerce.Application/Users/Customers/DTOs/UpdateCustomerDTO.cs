using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Users.Customers.DTOs;
public class UpdateCustomerDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CountryCode { get; set; }
    public CreateAddressDTO? Address { get; set; }
}
