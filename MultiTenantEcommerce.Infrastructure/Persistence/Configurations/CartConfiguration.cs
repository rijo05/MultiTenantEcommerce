using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    private readonly TenantContext _tenantContext;

    public CartConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });

        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey(x => new { x.TenantId, x.CartId })
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
