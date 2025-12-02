using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class Role : TenantBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsSystemRole { get; private set; }

    private readonly HashSet<RolePermission> _permissions = new();
    public IReadOnlyCollection<RolePermission> Permissions => _permissions;

    private Role() { }

    public Role(Guid tenantId, string name, string description)
        : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        Name = name;
        Description = description;
        IsSystemRole = false;
    }

    public void AddPermission(Guid permissionId)
    {
        CanItBeModifiedOrDeleted();

        if (_permissions.Any(x => x.PermissionId == permissionId)) return;

        _permissions.Add(new RolePermission(this.TenantId, this.Id, permissionId));
    }

    public void RemovePermission(Guid permissionId)
    {
        CanItBeModifiedOrDeleted();

        if (_permissions.Any(x => x.PermissionId == permissionId)) return;

        _permissions.Remove(new RolePermission(this.TenantId, this.Id, permissionId));
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
}
