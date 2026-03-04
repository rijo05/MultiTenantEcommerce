using MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Utilities.Guards;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

public class Role : TenantBase
{
    private readonly HashSet<RolePermission> _permissions = new();

    private Role()
    {
    }

    public Role(Guid tenantId, string name, string description)
        : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        Name = name;
        Description = description;
        IsSystemRole = false;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsSystemRole { get; private set; }
    public IReadOnlyCollection<RolePermission> Permissions => _permissions;

    public void AddPermission(Guid permissionId)
    {
        CanItBeModifiedOrDeleted();

        if (_permissions.Any(x => x.PermissionId == permissionId)) return;

        _permissions.Add(new RolePermission(TenantId, Id, permissionId));
    }

    public void RemovePermission(Guid permissionId)
    {
        CanItBeModifiedOrDeleted();

        if (_permissions.Any(x => x.PermissionId == permissionId)) return;

        _permissions.Remove(new RolePermission(TenantId, Id, permissionId));
    }

    public void CanItBeModifiedOrDeleted()
    {
        if (IsSystemRole)
            throw new Exception("This is a system role. Unable to make any changes to it");
    }

    public void MarkRoleAsSystemRole()
    {
        IsSystemRole = true;
    }

    public static Role CreateSystemOwner(Guid tenantId, IEnumerable<Permission> permissions)
    {
        var role = new Role(tenantId, SystemRoles.Owner, "Full access");

        foreach (var item in permissions)
        {
            role.AddPermission(item.Id);
        }

        return role;
    }

    public static Role CreateSystemAdmin(Guid tenantId, IEnumerable<Permission> permissions)
    {
        var role = new Role(tenantId, SystemRoles.Admin, "Full access except tenant settings");

        foreach (var item in permissions)
        {
            if(!item.Area.Equals("tenant", StringComparison.OrdinalIgnoreCase))
                role.AddPermission(item.Id);
        }

        return role;
    }

    #region update
    public void UpdateRole(string? name, string? description)
    {
        CanItBeModifiedOrDeleted();

        if (!string.IsNullOrEmpty(name))
            UpdateName(name);

        if (!string.IsNullOrEmpty(description))
            UpdateDescription(description);
    }

    private void UpdateName(string name)
    {
        Name = name;
        SetUpdatedAt();
    }

    private void UpdateDescription(string description)
    {
        Description = description;
        SetUpdatedAt();
    }
    #endregion
}