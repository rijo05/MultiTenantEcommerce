using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImages>
{
    public void Configure(EntityTypeBuilder<ProductImages> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Product>()
            .WithMany(p => p.Images)
            .HasForeignKey(x => new { x.TenantId, x.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();
    }
}