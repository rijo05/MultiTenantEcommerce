using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Configurations;

public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(p => p.PlanLimits, pl =>
        {
            pl.Property(x => x.MaxProducts).IsRequired();
            pl.Property(x => x.MaxCategories).IsRequired();
            pl.Property(x => x.MaxImagesPerProduct).IsRequired();
            pl.Property(x => x.MaxTenantMembers).IsRequired();
            pl.Property(x => x.MaxStorageBytes).IsRequired();
        });

        builder.HasMany(x => x.Prices)
            .WithOne(p => p.Plan)
            .HasForeignKey(p => p.PlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}