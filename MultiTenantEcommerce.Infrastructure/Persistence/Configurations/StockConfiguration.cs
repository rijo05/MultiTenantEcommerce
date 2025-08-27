using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    private readonly TenantContext _tenantContext;

    public StockConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Product>()
                .WithOne()
                .HasForeignKey<Stock>(s => new { s.TenantId, s.ProductId })
                .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(s => s.TenantId)
                .HasDatabaseName("IX_Stock_TenantId");

        builder.HasIndex(s => new { s.TenantId, s.ProductId })
                .HasDatabaseName("IX_Stock_TenantId_ProductId");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
