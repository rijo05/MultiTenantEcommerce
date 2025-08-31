using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;


namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerMapper _mapper;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, CustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDTO> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Customer not found.");

        return _mapper.ToCustomerResponseDTO(customer);
    }
}
