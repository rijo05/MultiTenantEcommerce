using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;
using MultiTenantEcommerce.Shared.Application.Auth;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.Security.Claims;

namespace MultiTenantEcommerce.Application.Platform.Identity.Commands.Auth.LoginUser;
public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthUserResponseDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public async Task<AuthUserResponseDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(new Email(request.Email));

        if (user == null || !_passwordHasher.Verify(request.Password, user.Password))
            throw new Exception("Email or password are wrong");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("userType", "user"),
            new("isPlatformAdmin", user.IsPlatformAdmin.ToString().ToLower(), ClaimValueTypes.Boolean),

            new("email_verified", user.EmailVerified.ToString().ToLower(), ClaimValueTypes.Boolean),
            new(JwtClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return new AuthUserResponseDTO(_tokenService.GenerateToken(claims), 
            user.Id, 
            user.FirstName, 
            user.Email.Value);
    }
}
