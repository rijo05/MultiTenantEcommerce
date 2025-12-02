using MultiTenantEcommerce.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class RolePermission : TenantBase
{
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }

    private RolePermission() { }

    public RolePermission(Guid tenantId, Guid roleId, Guid permissionId)
        :base(tenantId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

}
