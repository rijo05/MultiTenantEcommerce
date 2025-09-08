using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class EmployeeRole : TenantBase
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    private EmployeeRole() { }
    public EmployeeRole(Guid employeeId, Guid roleId)
    {
        EmployeeId = employeeId;
        RoleId = roleId;
    }
}
