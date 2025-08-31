using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    private readonly TenantContext _tenantContext;

    public OrderItemConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.OrderId, x.ProductId });


        builder.HasOne<Order>()
                .WithMany(x => x.Items)
                .HasForeignKey(x => new { x.TenantId, x.OrderId })
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => new { x.TenantId, x.ProductId })
                .OnDelete(DeleteBehavior.Restrict);


        builder.OwnsOne(u => u.UnitPrice, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("UnitPrice")
                .IsRequired();
        });


        builder.HasIndex(oi => oi.TenantId)
                .HasDatabaseName("IX_OrderItem_TenantId");

        builder.HasIndex(oi => new { oi.TenantId, oi.OrderId })
                .HasDatabaseName("IX_OrderItem_TenantId_OrderId");

        builder.HasIndex(oi => new { oi.TenantId, oi.ProductId })
                .HasDatabaseName("IX_OrderItem_TenantId_ProductId");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
