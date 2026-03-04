using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application.Auth;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.Security.Claims;
using MultiTenantEcommerce.Application.Platform.Identity.Auth.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.LoginCustomer;

public class LoginCustomerQueryHandler : IQueryHandler<LoginCustomerQuery, AuthCustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCustomerQueryHandler(ICustomerRepository customerRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher)
    {
        _customerRepository = customerRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthCustomerResponseDTO> Handle(LoginCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(new Email(request.Email));

        if (customer is null || !_passwordHasher.Verify(request.Password, customer.PasswordHash))
            throw new Exception("Email or password are wrong");

        //TODO() ###########
        //if (!customer.EmailVerified)
        //    throw new Exception("Please verify ur email");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new("tenantId", customer.TenantId.ToString()),
                new("userType", "customer"),
        
                new("email_verified", customer.EmailVerified.ToString().ToLower(), ClaimValueTypes.Boolean),
                new(JwtClaimNames.Jti, Guid.NewGuid().ToString())
            };

        return new AuthCustomerResponseDTO(_tokenService.GenerateToken(claims), 
            customer.Id, 
            customer.FirstName, 
            customer.Email.Value);
    }
}