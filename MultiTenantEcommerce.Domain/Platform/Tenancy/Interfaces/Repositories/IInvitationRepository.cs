using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
public interface IInvitationRepository : IRepository<TenantInvitation>
{
    public Task<TenantInvitation?> GetInvitationByTokenAsync(Guid Token);

    public Task<TenantInvitation?> GetPendingByEmail(Email Email);
}
