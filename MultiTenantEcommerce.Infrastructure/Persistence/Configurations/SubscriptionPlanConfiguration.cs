using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
{
    public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired();

        builder.Property(x => x.StripePriceId)
               .IsRequired();

        builder.Property(x => x.TransactionFee)
               .IsRequired();

        builder.OwnsOne(p => p.Price, money =>
        {
            money.Property(m => m.Value)
                 .HasColumnName("Price")
                 .IsRequired();
        });

        builder.OwnsOne(p => p.PlanLimits, pl =>
        {
            pl.Property(x => x.MaxProducts).IsRequired();
            pl.Property(x => x.MaxCategories).IsRequired();
            pl.Property(x => x.MaxImagesPerProduct).IsRequired();
            pl.Property(x => x.MaxEmployees).IsRequired();
            pl.Property(x => x.MaxStorageBytes).IsRequired();
        });
    }
}
