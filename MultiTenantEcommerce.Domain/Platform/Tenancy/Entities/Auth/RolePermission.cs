using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

public class RolePermission : TenantBase
{
    private RolePermission()
    {
    }

    public RolePermission(Guid tenantId, Guid roleId, Guid permissionId)
        : base(tenantId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }
    public Permission Permission { get; private set; }
}