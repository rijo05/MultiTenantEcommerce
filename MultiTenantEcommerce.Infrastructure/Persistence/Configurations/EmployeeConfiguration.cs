using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    private readonly TenantContext _tenantContext;

    public EmployeeConfiguration(TenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

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
            password.Property(e => e.Value)
                .HasColumnName("Password")
                .IsRequired();
        });

        builder.OwnsOne(u => u.Role, role =>
        {
            role.Property(r => r.roleName)
                .HasConversion<string>()
                .HasColumnName("Role")
                .IsRequired();
        });




        builder.HasIndex(e => e.TenantId)
                .HasDatabaseName("IX_Employee_TenantId");

        builder.HasIndex("TenantId", "Email")
            .IsUnique();

        //builder.HasIndex(e => new { e.TenantId, e.Email.Value })
        //        .HasDatabaseName("IX_Employee_TenantId_Email");

        //builder.HasIndex(e => new { e.TenantId, e.Role.roleName })
        //    .HasDatabaseName("IX_Employee_TenantId_Role");

        builder.HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }
}
