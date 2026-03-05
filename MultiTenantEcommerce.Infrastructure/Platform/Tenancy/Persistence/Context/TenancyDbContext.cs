using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;

public class TenancyDbContext : ModuleDbContext
{
    public TenancyDbContext(DbContextOptions<TenancyDbContext> options,
        ITenantContext tenantContext) : base(options, tenantContext)
    {
    }

    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<TenantSubscription> TenantSubscriptions { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<TenantMemberRole> TenantMemberRoles { get; set; }
    public virtual DbSet<TenantInvitation> TenantInvitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("tenancy");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenancyDbContext).Assembly);

        modelBuilder.Entity<Role>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantMemberRole>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantInvitation>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<RolePermission>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantMember>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
