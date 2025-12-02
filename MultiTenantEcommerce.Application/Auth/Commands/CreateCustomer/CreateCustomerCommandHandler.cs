using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateCustomer;
public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, AuthCustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;
    private readonly ITokenService _tokenService;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext,
        ITokenService tokenService)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _tokenService = tokenService;
    }

    public async Task<AuthCustomerResponseDTO> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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

        return new AuthCustomerResponseDTO
        {
            Id = customer.Id,
            Email = customer.Email.Value,
            Name = customer.Name,
            Token = _tokenService.GenerateToken(customer, new List<string> { }, new List<string> { })
        };
    }
}
