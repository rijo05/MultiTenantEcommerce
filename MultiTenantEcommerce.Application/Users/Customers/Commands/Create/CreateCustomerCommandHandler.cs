using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Create;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CustomerMapper _mapper;
    private readonly TenantContext _tenantContext;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        CustomerMapper mapper,
        TenantContext tenantContext)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerResponseDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.GetByEmailAsync(new Email(request.Email)) != null)
            throw new Exception("Customer with this email already exists.");

        if (await _customerRepository.GetByPhoneNumberAsync(new PhoneNumber(request.CountryCode, request.PhoneNumber)) != null)
            throw new Exception("Customer with this phone number already exists.");

        var customer = new Domain.Users.Entities.Customer(
            _tenantContext.TenantId,
            request.Name,
            new Email(request.Email),
            new Password(request.Password),
            new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.PostalCode,
                request.Address.Country,
                request.Address.HouseNumber),
            new PhoneNumber(request.CountryCode, request.PhoneNumber)
        );

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        return _mapper.ToCustomerResponseDTO(customer);
    }
}
