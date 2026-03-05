using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
public class CatalogDbContext : ModuleDbContext
{
    private readonly ITenantContext _tenantContext;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options, 
        ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<ProductImages> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("catalog");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

        modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ProductImages>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
