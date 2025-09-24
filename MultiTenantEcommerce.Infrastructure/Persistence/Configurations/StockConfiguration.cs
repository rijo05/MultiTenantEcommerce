using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;

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

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken()
            .ValueGeneratedOnAdd();

        builder.OwnsOne(x => x.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .IsRequired();
        });
        builder.OwnsOne(x => x.Reserved, rsv =>
        {
            rsv.Property(q => q.Value)
               .HasColumnName("ReservedStock")
               .IsRequired();
        });
        builder.OwnsOne(x => x.MinimumQuantity, minqty =>
        {
            minqty.Property(q => q.Value)
               .HasColumnName("MinimumQuantity")
               .IsRequired();
        });

        //builder.HasIndex(s => s.TenantId)
        //        .HasDatabaseName("IX_Stock_TenantId");

        //builder.HasIndex(s => new { s.TenantId, s.ProductId })
        //        .HasDatabaseName("IX_Stock_TenantId_ProductId");
    }
}
