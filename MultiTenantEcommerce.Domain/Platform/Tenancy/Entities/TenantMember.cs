using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

public class TenantMember : TenantBase
{
    public Guid UserId { get; private set; }
    public bool IsOwner { get; }
    public IReadOnlyCollection<TenantMemberRole> TenantMemberRoles => _roles;
    private readonly HashSet<TenantMemberRole> _roles = new();

    private TenantMember()
    {
    }

    public TenantMember(Guid tenantId, Guid userId, bool isOwner)
        : base(tenantId)
    {
        UserId = userId;
        IsOwner = isOwner;

        AddDomainEvent(new TenantMemberRegisteredEvent(TenantId, Id));
    }


    public void AddRole(Guid roleId)
    {
        if (_roles.Any(x => x.RoleId == roleId))
            return;

        _roles.Add(new TenantMemberRole(TenantId, Id, roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        var employeeRole = _roles.FirstOrDefault(x => x.RoleId == roleId);

        if (employeeRole != null)
            _roles.Remove(employeeRole);
    }

    public bool HasPermission(string permissionName)
    {
        if (IsOwner) return true;

        return _roles.Select(mr => mr.Role)
            .SelectMany(r => r.Permissions)
            .Any(p => p.Permission.Name == permissionName);
    }
}