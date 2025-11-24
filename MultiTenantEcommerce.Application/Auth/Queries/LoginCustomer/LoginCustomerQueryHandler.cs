using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Queries.LoginCustomer;
public class LoginCustomerQueryHandler : IQueryHandler<LoginCustomerQuery, AuthCustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;

    public LoginCustomerQueryHandler(ICustomerRepository customerRepository,
        ITokenService tokenService)
    {
        _customerRepository = customerRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthCustomerResponseDTO> Handle(LoginCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(new Email(request.Email));

        if (customer is null || !customer.Password.VerifySamePassword(request.Password))
            throw new Exception("Email or password are wrong");

        return new AuthCustomerResponseDTO
        {
            Email = request.Email,
            Id = customer.Id,
            Name = customer.Name,
            Token = _tokenService.CreateSessionToken(customer)
        };
    }
}
