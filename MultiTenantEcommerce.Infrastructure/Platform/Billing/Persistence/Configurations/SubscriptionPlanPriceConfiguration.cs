using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Configurations;

public class SubscriptionPlanPriceConfiguration : IEntityTypeConfiguration<SubscriptionPlanPrice>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlanPrice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(u => u.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired();
        });
    }
}