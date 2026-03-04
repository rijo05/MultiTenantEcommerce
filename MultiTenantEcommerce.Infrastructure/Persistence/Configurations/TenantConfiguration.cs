using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired();
        });

        builder.HasMany(t => t.ShippingProviders)
            .WithOne()
            .HasForeignKey("TenantId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Subscription)
            .WithOne()
            .HasForeignKey<TenantSubscription>("TenantId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}