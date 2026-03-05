using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Context;

public class BillingDbContext : ModuleDbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options, ITenantContext tenantContext) : base(options, tenantContext)
    {
    }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public virtual DbSet<SubscriptionPlanPrice> SubscriptionPlanPrices { get; set; }

    protected override bool RequiresTenant => false;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("billing");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
    }
}
