using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class EmployeeRole : TenantBase
{
    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }

    private EmployeeRole() { }
    public EmployeeRole(Guid tenantId, Guid employeeId, Guid roleId) 
        :base(tenantId)
    {
        EmployeeId = employeeId;
        RoleId = roleId;
    }
}
