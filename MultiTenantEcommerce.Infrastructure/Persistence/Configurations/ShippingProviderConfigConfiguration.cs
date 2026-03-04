using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class ShippingProviderConfigConfiguration : IEntityTypeConfiguration<ShippingProviderConfig>
{
    public void Configure(EntityTypeBuilder<ShippingProviderConfig> builder)
    {
    }
}