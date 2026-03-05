using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Context;
public class CustomerDbContext : ModuleDbContext
{
    public CustomerDbContext(DbContextOptions options, ITenantContext tenantContext) : base(options, tenantContext)
    {
    }
    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("customer");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);

        modelBuilder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
