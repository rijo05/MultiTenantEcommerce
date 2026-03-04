using System.Security.Claims;

namespace MultiTenantEcommerce.Shared.Application.Auth;

public interface ITokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
    ClaimsPrincipal ValidateToken(string token);
}