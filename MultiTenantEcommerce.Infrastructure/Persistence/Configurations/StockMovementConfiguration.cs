using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
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


        //builder.HasIndex(sm => sm.TenantId)
        //        .HasDatabaseName("IX_StockMovement_TenantId");

        //builder.HasIndex(sm => new { sm.TenantId, sm.ProductId })
        //        .HasDatabaseName("IX_StockMovement_TenantId_ProductId");
    }
}
