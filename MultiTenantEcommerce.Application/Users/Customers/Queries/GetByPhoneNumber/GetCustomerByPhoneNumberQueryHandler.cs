using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetByPhoneNumber;
public class GetCustomerByPhoneNumberQueryHandler : IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerMapper _mapper;

    public GetCustomerByPhoneNumberQueryHandler(ICustomerRepository customerRepository, CustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDTO> Handle(GetCustomerByPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByPhoneNumberAsync(new PhoneNumber(request.CountryCode, request.Number))
            ?? throw new Exception($"Customer with phone number {request.Number} not found.");

        return _mapper.ToCustomerResponseDTO(customer);
    }
}
