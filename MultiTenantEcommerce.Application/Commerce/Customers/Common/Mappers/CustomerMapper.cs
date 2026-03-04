using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Shared.Application;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;

public static class CustomerMapper
{
    public static CustomerResponseDTO ToDTO(this Customer customer)
    {
        return new CustomerResponseDTO(
            customer.Id,
            customer.FirstName,
            customer.Email.Value,
            customer.Addresses.ToDTOList(customer.DefaultAddressId),
            customer.CreatedAt,
            customer.UpdatedAt
        );
    }

    public static List<CustomerResponseDTO> ToDTOList(this IEnumerable<Customer> customers)
    {
        return customers.Select(x => x.ToDTO()).ToList();
    }

    public static PaginatedList<CustomerResponseDTO> ToPaginatedDTOAdmin(this PaginatedList<Customer> paginatedCustomers)
    {
        var dtoList = ToDTOList(paginatedCustomers.Items);

        return new PaginatedList<CustomerResponseDTO>(
            dtoList,
            paginatedCustomers.TotalCount,
            paginatedCustomers.Page,
            paginatedCustomers.PageSize
        );
    }
}