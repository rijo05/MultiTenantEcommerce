namespace MultiTenantEcommerce.Application.DTOs.Address;

public class CreateAddressDTO
{
    public string Street { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string HouseNumber { get; set; }
}
