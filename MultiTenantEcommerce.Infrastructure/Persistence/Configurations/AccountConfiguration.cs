using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Account>(x => x.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Tenants)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}