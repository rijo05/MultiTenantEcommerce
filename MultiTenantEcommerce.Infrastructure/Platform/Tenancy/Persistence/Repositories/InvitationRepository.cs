using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;


namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;
public class InvitationRepository : Repository<TenantInvitation>, IInvitationRepository
{
    public InvitationRepository(TenancyDbContext appDbContext) : base(appDbContext)
    {
    }
    public async Task<TenantInvitation?> GetInvitationByTokenAsync(Guid Token)
    {
        return await _dbContext.Set<TenantInvitation>()
            .FirstOrDefaultAsync(x => x.Token == Token);
    }

    public async Task<TenantInvitation?> GetPendingByEmail(Email Email)
    {
        return await _dbContext.Set<TenantInvitation>()
            .FirstOrDefaultAsync(x => x.Email.Value == Email.Value && 
            !x.IsAccepted && 
            x.ExpiresAt > DateTime.UtcNow);
    }
}
