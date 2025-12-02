using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
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

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property<string>("Value")
                .HasColumnName("Password")
                .IsRequired();
        });


        //builder.HasIndex(e => e.TenantId)
        //        .HasDatabaseName("IX_Employee_TenantId");

        //builder.HasIndex("TenantId", "Email")
        //    .IsUnique();

        //builder.HasIndex(e => new { e.TenantId, e.Email.Value })
        //        .HasDatabaseName("IX_Employee_TenantId_Email");

        //builder.HasIndex(e => new { e.TenantId, e.Role.roleName })
        //    .HasDatabaseName("IX_Employee_TenantId_Role");
    }
}
