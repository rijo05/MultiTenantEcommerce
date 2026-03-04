using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Notifications.Entities;
using MultiTenantEcommerce.Domain.Notifications.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

internal class TenantNotificationProfileConfiguration : IEntityTypeConfiguration<TenantNotificationProfile>
{
    public void Configure(EntityTypeBuilder<TenantNotificationProfile> builder)
    {
        builder.OwnsOne(t => t.Theme, theme =>
        {
            theme.Property(t => t.PrimaryColor)
                .HasConversion(c => c.Value, v => HexColor.Create(v).Value)
                .HasColumnName("PrimaryColor");

            theme.Property(t => t.SecondaryColor)
                .HasConversion(c => c.Value, v => HexColor.Create(v).Value)
                .HasColumnName("SecondaryColor");

            theme.Property(t => t.LogoUrl).HasColumnName("LogoUrl");
        });

        builder.HasMany(t => t.Overrides).WithOne().HasForeignKey("ProfileId").OnDelete(DeleteBehavior.Cascade);
    }
}