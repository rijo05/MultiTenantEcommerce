using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> builder)
    {
        builder.HasKey(er => new { er.TenantId, er.EmployeeId, er.RoleId });

        builder.HasOne(er => er.Employee)
               .WithMany(e => e.EmployeeRoles)
               .HasForeignKey(er => new { er.TenantId, er.EmployeeId })
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(er => er.Role)
               .WithMany()
               .HasForeignKey(er => new { er.TenantId, er.RoleId })
               .OnDelete(DeleteBehavior.Restrict);
    }
}
