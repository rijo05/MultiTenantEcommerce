using MediatR;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponseDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id)
                       ?? throw new Exception("Customer not found.");

        return customer.ToDTO();
    }
}