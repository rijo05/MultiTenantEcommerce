using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.CartId, x.ProductId });

        builder.HasOne<Cart>()
                .WithMany(c => c.Items)
                .HasForeignKey(ci => new { ci.TenantId, ci.CartId });

        builder.OwnsOne(x => x.Quantity, qty =>
        {
            qty.Property(q => q.Value)
               .HasColumnName("Quantity")
               .IsRequired();
        });
    }
}
