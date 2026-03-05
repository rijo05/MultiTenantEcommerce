using MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Infrastructure.Platform.Identity;
internal class IdentityIntegrationProxy : IIdentityIntegrationProxy
{
    private readonly IUserRepository _userRepository;

    public IdentityIntegrationProxy(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid?> GetUserIdByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(new Email(email));
        return user?.Id;
    }
}
