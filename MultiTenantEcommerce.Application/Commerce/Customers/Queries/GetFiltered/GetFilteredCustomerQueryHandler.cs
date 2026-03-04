using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetFiltered;

public class GetFilteredCustomerQueryHandler : IQueryHandler<GetFilteredCustomerQuery, PaginatedList<CustomerResponseDTO>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetFilteredCustomerQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<PaginatedList<CustomerResponseDTO>> Handle(GetFilteredCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.GetFilteredAsync(
            request.Name,
            request.Email,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return customers.ToPaginatedDTOAdmin();
    }
}