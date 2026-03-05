using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Context;

public class IdentityDbContext : ModuleDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ITenantContext tenantContext) : base(options, tenantContext)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override bool RequiresTenant => false;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("identity");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }
}
