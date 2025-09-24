using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => new { r.TenantId, r.Id });

        builder.HasOne<Tenant>()
               .WithMany()
               .HasForeignKey(r => r.TenantId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Permissions)
                .WithMany();
    }
}
