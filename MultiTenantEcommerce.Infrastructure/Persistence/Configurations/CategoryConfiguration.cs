using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    private readonly TenantContext _tenantContext;

    public CategoryConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }


    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });


        builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

        //builder.HasIndex(c => c.TenantId)
        //        .HasDatabaseName("IX_Category_TenantId");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
