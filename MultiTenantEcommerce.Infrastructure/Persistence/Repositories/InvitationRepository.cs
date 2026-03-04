using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;


namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class InvitationRepository : Repository<TenantInvitation>, IInvitationRepository
{
    public InvitationRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }
    public async Task<TenantInvitation?> GetInvitationByTokenAsync(Guid Token)
    {
        return await _appDbContext.TenantInvitations
            .FirstOrDefaultAsync(x => x.Token == Token);
    }

    public async Task<TenantInvitation?> GetPendingByEmail(Email Email)
    {
        return await _appDbContext.TenantInvitations
            .FirstOrDefaultAsync(x => x.Email.Value == Email.Value && 
            !x.IsAccepted && 
            x.ExpiresAt > DateTime.UtcNow);
    }
}
