using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
public interface IInvitationRepository : IRepository<TenantInvitation>
{
    public Task<TenantInvitation?> GetInvitationByTokenAsync(Guid Token);

    public Task<TenantInvitation?> GetPendingByEmail(Email Email);
}
