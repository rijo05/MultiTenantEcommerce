using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });


        builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => new { x.TenantId, x.CustomerId })
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Items)
               .WithOne()
               .HasForeignKey(oi => new { oi.TenantId, oi.OrderId })
               .OnDelete(DeleteBehavior.Cascade);


        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").IsRequired();
            address.Property(a => a.HouseNumber).HasColumnName("HouseNumber").IsRequired();
            address.Property(a => a.City).HasColumnName("City").IsRequired();
            address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
            address.Property(a => a.Country).HasColumnName("Country").IsRequired();
        });

        builder.Property(o => o.OrderStatus)
            .HasConversion<string>()
            .IsRequired();


        builder.OwnsOne(u => u.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired();
        });

        //builder.HasIndex(o => o.TenantId)
        //        .HasDatabaseName("IX_Order_TenantId");

        //builder.HasIndex(o => new { o.TenantId, o.CustomerId })
        //        .HasDatabaseName("IX_Order_TenantId_CustomerId");
    }
}
