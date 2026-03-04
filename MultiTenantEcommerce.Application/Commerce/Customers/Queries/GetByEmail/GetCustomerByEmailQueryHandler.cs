using MediatR;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetByEmail;

public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerResponseDTO> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(new Email(request.Email))
                       ?? throw new Exception($"Customer with email {request.Email} not found.");

        return customer.ToDTO();
    }
}