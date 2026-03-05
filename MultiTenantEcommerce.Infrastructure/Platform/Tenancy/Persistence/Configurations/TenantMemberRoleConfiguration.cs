using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Configurations;

public class TenantMemberRoleConfiguration : IEntityTypeConfiguration<TenantMemberRole>
{
    public void Configure(EntityTypeBuilder<TenantMemberRole> builder)
    {
        builder.HasKey(er => new { er.TenantId, er.TenantMemberId, er.RoleId });

        builder.HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}