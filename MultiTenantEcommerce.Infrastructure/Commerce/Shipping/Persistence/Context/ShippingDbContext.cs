using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Context;

public class ShippingDbContext : ModuleDbContext
{
    public ShippingDbContext(DbContextOptions options, ITenantContext tenantContext) : base(options, tenantContext)
    {
    }
    public virtual DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("shipping");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShippingDbContext).Assembly);

        modelBuilder.Entity<Shipment>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ShippingProviderConfig>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

    }
}
