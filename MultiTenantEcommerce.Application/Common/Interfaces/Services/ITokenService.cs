using MultiTenantEcommerce.Domain.Users.Entities;
using System.Security.Claims;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface ITokenService
{
    string GenerateToken(UserBase user, List<string> roles, List<string> permissions);
    ClaimsPrincipal ValidateToken(string token);
}
