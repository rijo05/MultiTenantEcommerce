using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging.Abstractions;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    private readonly TenantContext _tenantContext;

    public StockMovementConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });


        builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => new { x.TenantId, x.ProductId })
                .OnDelete(DeleteBehavior.Restrict);


        builder.Property(o => o.Reason)
                .HasConversion<string>()
                .IsRequired();


        builder.HasIndex(sm => sm.TenantId)
                .HasDatabaseName("IX_StockMovement_TenantId");

        builder.HasIndex(sm => new { sm.TenantId, sm.ProductId })
                .HasDatabaseName("IX_StockMovement_TenantId_ProductId");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
