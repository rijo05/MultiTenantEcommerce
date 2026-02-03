using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Product>()
                .WithOne()
                .HasForeignKey<Stock>(s => new { s.TenantId, s.ProductId })
                .OnDelete(DeleteBehavior.Restrict);


        builder.Property(x => x.Quantity)
            .HasConversion(
                vo => vo.Value,
                dbValue => new NonNegativeQuantity(dbValue))
            .IsRequired();

        builder.Property(x => x.MinimumQuantity)
            .HasConversion(
            vo => vo.Value,
            db => new NonNegativeQuantity(db));

        builder.Property(x => x.Reserved)
            .HasConversion(
            vo => vo.Value,
            db => new NonNegativeQuantity(db));

        //builder.HasIndex(s => s.TenantId)
        //        .HasDatabaseName("IX_Stock_TenantId");

        //builder.HasIndex(s => new { s.TenantId, s.ProductId })
        //        .HasDatabaseName("IX_Stock_TenantId_ProductId");
    }
}
