using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Configurations;

public class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
{
    public void Configure(EntityTypeBuilder<TenantSubscription> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Plan)
            .WithMany()
            .HasForeignKey(x => x.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PlanPrice)
            .WithMany()
            .HasForeignKey(x => x.PlanPriceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}