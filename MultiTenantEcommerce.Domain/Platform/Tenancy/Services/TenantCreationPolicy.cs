using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Services;
public class TenantCreationPolicy : ITenantCreationPolicy
{
    private readonly ITenantMemberRepository _tenantMemberRepository;

    public TenantCreationPolicy(ITenantMemberRepository tenantMemberRepository)
    {
        _tenantMemberRepository = tenantMemberRepository;
    }

    public async Task EnsureUserCanCreateTenantAsync(Guid userId)
    {
        if (await _tenantMemberRepository.CountTrialStoresForOwnerAsync(userId) >= 3)
            throw new Exception("Trial limit exceeded");
    }
}
