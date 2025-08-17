using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Configurations;
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    private readonly TenantContext _tenantContext;

    public CustomerConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => new { x.TenantId, x.Id });


        builder.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);


        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired();
        });


        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").IsRequired();
            address.Property(a => a.HouseNumber).HasColumnName("HouseNumber").IsRequired();
            address.Property(a => a.City).HasColumnName("City").IsRequired();
            address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
            address.Property(a => a.Country).HasColumnName("Country").IsRequired();
        });

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(e => e.Value)
                .HasColumnName("Password")
                .IsRequired();
        });

        builder.OwnsOne(u => u.PhoneNumber, phoneNumber =>
        {
            phoneNumber.Property(e => e.CountryCode)
                .HasColumnName("PhoneNumber_CountryCode")
                .IsRequired();

            phoneNumber.Property(e => e.Number)
                .HasColumnName("PhoneNumber_Number")
                .IsRequired();
        });


        builder.HasIndex(c => c.TenantId)
                .HasDatabaseName("IX_Customer_TenantId");

        //builder.HasIndex(c => new { c.TenantId, c.Email.Value })
        //        .HasDatabaseName("IX_Customer_TenantId_Email");

        //builder.HasIndex(c => new { c.TenantId, c.PhoneNumber.Number })
        //        .HasDatabaseName("IX_Customer_TenantId_PhoneNumber");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
