using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
{
    public void Configure(EntityTypeBuilder<OrderPayment> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(u => u.Amount, amount =>
        {
            amount.Property(e => e.Value)
                .HasColumnName("Amount")
                .IsRequired();
        });

        builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

        builder.Property(x => x.PaymentMethod)
               .HasConversion<string>()
               .IsRequired();
    }
}
