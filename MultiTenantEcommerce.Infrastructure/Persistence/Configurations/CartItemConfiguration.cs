using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    private readonly TenantContext _tenantContext;

    public CartItemConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.CartId, x.ProductId });

        builder.HasOne<Cart>()
                .WithMany(c => c.Items)
                .HasForeignKey(ci => new { ci.TenantId, ci.CartId });

        builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => new { x.TenantId, x.ProductId });

        builder.OwnsOne(x => x.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .IsRequired();
        });

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
