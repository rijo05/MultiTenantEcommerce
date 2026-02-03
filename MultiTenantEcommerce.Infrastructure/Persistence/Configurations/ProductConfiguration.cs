using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(u => u.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired();
        });

        builder.HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => new { x.TenantId, x.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.CategoryId })
            .OnDelete(DeleteBehavior.Restrict);


        //builder.HasIndex(p => p.TenantId)
        //        .HasDatabaseName("IX_Product_TenantId");

        //builder.HasIndex(p => new { p.TenantId, p.CategoryId })
        //        .HasDatabaseName("IX_Product_TenantId_CategoryId");

        //builder.HasIndex(p => new { p.TenantId, p.Name })
        //        .HasDatabaseName("IX_Product_TenantId_ProductName");
    }
}
