using MultiTenantEcommerce.Application.Common.Mappers;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Domain.Users.Entities;

namespace MultiTenantEcommerce.Application.Users.Customers.Mappers;
public class CustomerMapper
{
    private readonly AddressMapper _addressMapper;

    public CustomerMapper(AddressMapper addressMapper)
    {
        _addressMapper = addressMapper;
    }

    public CustomerResponseDTO ToCustomerResponseDTO(Customer customer)
    {
        return new CustomerResponseDTO()
        {
            Address = _addressMapper.ToAddressResponseFromDTO(customer.Address),
            CountryCode = customer.PhoneNumber.CountryCode,
            PhoneNumber = customer.PhoneNumber.Number,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            Email = customer.Email.Value,
            Id = customer.Id,
            Name = customer.Name
        };
    }

    public List<CustomerResponseDTO> ToCustomerResponseDTOList(List<Customer> Customer)
    {
        return Customer.Select(x => ToCustomerResponseDTO(x)).ToList();
    }
}
