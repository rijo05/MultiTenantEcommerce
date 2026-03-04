using MultiTenantEcommerce.Domain.Notifications.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Notifications.Interfaces;

public interface ITenantNotificationProfileRepository : IRepository<TenantNotificationProfile>
{
    Task<TenantNotificationProfile?> GetByTenantIdAsync(Guid tenantId);
}