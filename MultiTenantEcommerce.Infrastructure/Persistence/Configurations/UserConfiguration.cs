using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(255);

            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property<string>("Value")
                .HasColumnName("Password")
                .IsRequired();
        });
    }
}