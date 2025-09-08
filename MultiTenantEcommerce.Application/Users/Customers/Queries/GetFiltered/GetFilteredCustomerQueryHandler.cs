using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetFiltered;
public class GetFilteredCustomerQueryHandler : IQueryHandler<GetFilteredCustomerQuery, List<CustomerResponseDTO>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerMapper _customerMapper;

    public GetFilteredCustomerQueryHandler(ICustomerRepository customerRepository, CustomerMapper customerMapper)
    {
        _customerRepository = customerRepository;
        _customerMapper = customerMapper;
    }

    public async Task<List<CustomerResponseDTO>> Handle(GetFilteredCustomerQuery request, CancellationToken cancellationToken)
    {
        var employees = await _customerRepository.GetFilteredAsync(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return _customerMapper.ToCustomerResponseDTOList(employees);
    }
}
