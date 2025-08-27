using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Sales.Entities;
using MultiTenantEcommerce.Domain.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    private readonly TenantContext _tenantContext;

    public OrderConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

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

        builder.Property(o => o.PaymentMethod)
            .HasConversion<string>()
            .IsRequired();

        builder.OwnsOne(u => u.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired();
        });

        builder.HasIndex(o => o.TenantId)
                .HasDatabaseName("IX_Order_TenantId");

        builder.HasIndex(o => new { o.TenantId, o.CustomerId })
                .HasDatabaseName("IX_Order_TenantId_CustomerId");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
