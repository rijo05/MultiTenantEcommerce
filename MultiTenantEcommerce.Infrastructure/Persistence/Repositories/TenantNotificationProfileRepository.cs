using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Notifications.Entities;
using MultiTenantEcommerce.Domain.Notifications.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class TenantNotificationProfileRepository : Repository<TenantNotificationProfile>,
    ITenantNotificationProfileRepository
{
    public TenantNotificationProfileRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TenantNotificationProfile?> GetByTenantIdAsync(Guid tenantId)
    {
        return await _appDbContext.NotificationProfiles
            .Include(profile => profile.Overrides)
            .FirstOrDefaultAsync(profile => profile.TenantId == tenantId);
    }
}