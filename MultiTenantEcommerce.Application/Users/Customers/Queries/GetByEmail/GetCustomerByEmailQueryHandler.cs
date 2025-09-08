using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetByEmail;
public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerMapper _mapper;

    public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository, CustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDTO> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(new Email(request.Email))
            ?? throw new Exception($"Customer with email {request.Email} not found.");

        return _mapper.ToCustomerResponseDTO(customer);
    }
}
