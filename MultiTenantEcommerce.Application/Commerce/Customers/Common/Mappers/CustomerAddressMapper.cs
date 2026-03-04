using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;

public static class CustomerAddressMapper
{
    public static AddressResponseDTO ToDTO(this CustomerAddress address, Guid? defaultAddressId)
    {
        return new AddressResponseDTO(
            address.Id,
            address.Street,
            address.City,
            address.PostalCode,
            address.Country,
            address.HouseNumber,
            address.Id == defaultAddressId
        );
    }

    public static List<AddressResponseDTO> ToDTOList(this IEnumerable<CustomerAddress> addresses, Guid? defaultAddressId)
    {
        return addresses.Select(x => x.ToDTO(defaultAddressId)).ToList();
    }
}