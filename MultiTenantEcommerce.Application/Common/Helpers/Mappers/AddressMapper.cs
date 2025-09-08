using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.Helpers.Mappers;
public class AddressMapper
{
    public AddressResponseDTO ToAddressResponseFromDTO(Address address)
    {
        return new AddressResponseDTO
        {
            Street = address.Street,
            City = address.City,
            Country = address.Country,
            HouseNumber = address.HouseNumber,
            PostalCode = address.PostalCode
        };
    }

}
