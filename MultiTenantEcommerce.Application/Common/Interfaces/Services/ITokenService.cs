using MultiTenantEcommerce.Domain.Users.Entities;
using System.Security.Claims;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface ITokenService
{
    string CreateSessionToken(UserBase user);
    string CreateImageToken(Guid ProductId, Guid TenantId);
    ClaimsPrincipal ValidateToken(string token);
}
