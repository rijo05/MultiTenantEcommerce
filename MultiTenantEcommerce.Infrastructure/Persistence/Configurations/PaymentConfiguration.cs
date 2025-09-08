using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    private readonly TenantContext _tenantContext;

    public PaymentConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public void Configure(EntityTypeBuilder<Payment> builder)
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

        builder.Property(x => x.Reason)
                .HasConversion<string>()
                .IsRequired();

        builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

        builder.Property(x => x.PaymentMethod)
               .HasConversion<string>()
               .IsRequired();

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
