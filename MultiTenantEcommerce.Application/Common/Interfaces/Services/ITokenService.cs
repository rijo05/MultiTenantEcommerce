using MultiTenantEcommerce.Domain.Users.Entities;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface ITokenService
{
    string CreateToken(UserBase user);
}
