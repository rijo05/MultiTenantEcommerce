using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class Role : TenantBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsSystemRole { get; private set; }

    private readonly HashSet<Permission> _permissions = new();
    public IReadOnlyCollection<Permission> Permissions => _permissions;

    private Role() { }

    public Role(string name, string description, Guid tenantId)
        : base(tenantId)
    {
        Name = name;
        Description = description;
        IsSystemRole = false;
    }

    public void AddPermission(Permission permission)
    {
        CanItBeModifiedOrDeleted();

        _permissions.Add(permission);
    }

    public void RemovePermission(Permission permission)
    {
        CanItBeModifiedOrDeleted();

        _permissions.Remove(permission);
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
