using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    private readonly TenantContext _tenantContext;

    public RoleConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => new { r.TenantId, r.Id });

        builder.HasOne<Tenant>()
               .WithMany()
               .HasForeignKey(r => r.TenantId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Permissions)
                .WithMany();

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
