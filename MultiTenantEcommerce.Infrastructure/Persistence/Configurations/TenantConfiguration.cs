using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;

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

        builder.OwnsOne(x => x.Subscription, subscription =>
        {
            subscription.Property(s => s.PlanId)
                .HasColumnName("SubscriptionPlanId")
                .IsRequired();

            subscription.Property(s => s.StripeSubscriptionId)
                .HasColumnName("StripeSubscriptionId");

            subscription.Property(s => s.Status)
                .HasColumnName("SubscriptionStatus")
                .HasConversion<string>()
                .IsRequired();

            subscription.Property(s => s.CurrentPeriodStart)
                .HasColumnName("SubscriptionCurrentPeriodStart")
                .IsRequired();

            subscription.Property(s => s.CurrentPeriodEnd)
                .HasColumnName("SubscriptionCurrentPeriodEnd")
                .IsRequired();

            subscription.Property(s => s.CancelAtPeriodEnd)
                .HasColumnName("SubscriptionCancelAtPeriodEnd")
                .IsRequired();

            subscription.HasOne(s => s.Plan)
                .WithMany()
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
