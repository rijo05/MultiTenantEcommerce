using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").IsRequired();
            address.Property(a => a.HouseNumber).HasColumnName("HouseNumber").IsRequired();
            address.Property(a => a.City).HasColumnName("City").IsRequired();
            address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
            address.Property(a => a.Country).HasColumnName("Country").IsRequired();
        });

        builder.OwnsOne(u => u.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired();
        });
    }
}
