using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Configurations;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMember>
{
    public void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });


        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TenantMemberRoles)
            .WithOne()
            .HasForeignKey(r => new { r.TenantId, r.TenantMemberId })
            .OnDelete(DeleteBehavior.Cascade);


        //builder.HasIndex(e => e.TenantId)
        //        .HasDatabaseName("IX_TenantMember_TenantId");

        //builder.HasIndex("TenantId", "Email")
        //    .IsUnique();

        //builder.HasIndex(e => new { e.TenantId, e.Email.Value })
        //        .HasDatabaseName("IX_TenantMember_TenantId_Email");

        //builder.HasIndex(e => new { e.TenantId, e.Role.roleName })
        //    .HasDatabaseName("IX_TenantMember_TenantId_Role");
    }
}