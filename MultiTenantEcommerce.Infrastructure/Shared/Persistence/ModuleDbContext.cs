using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MultiTenantEcommerce.Infrastructure.Outbox;
using MultiTenantEcommerce.Infrastructure.Shared.Outbox;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Shared.Persistence;
public abstract class ModuleDbContext : DbContext
{
    protected readonly ITenantContext _tenantContext;
    protected ModuleDbContext(DbContextOptions options, ITenantContext tenantContext) : base(options) 
    {
        _tenantContext = tenantContext;
    }

    public virtual DbSet<OutboxEvent> OutboxEvents { get; set; }
    protected virtual bool RequiresTenant => true;

    public IEnumerable<IDomainEvent> GetAllDomainEvents()
    {
        return ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        ChangeTracker
            .Entries<BaseEntity>()
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var tenantId = _tenantContext.TenantId;
        var transactionTime = DateTime.UtcNow;

        if (tenantId == Guid.Empty) throw new InvalidOperationException("TenantId is not set in the TenantContext.");

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreatedAt(transactionTime);

                if (entry.Entity is TenantBase tenantEntity)
                {
                    if (tenantId == Guid.Empty)
                        throw new InvalidOperationException("TenantId is missing");

                    tenantEntity.SetTenantId(tenantId);
                }
            }
            else if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdatedAt(transactionTime);
        }

        return await base.SaveChangesAsync();
    }
}
