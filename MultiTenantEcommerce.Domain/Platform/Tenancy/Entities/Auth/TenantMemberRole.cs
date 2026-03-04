using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

public class TenantMemberRole : TenantBase
{
    private TenantMemberRole()
    {
    }

    public TenantMemberRole(Guid tenantId, Guid tenantMemberId, Guid roleId)
        : base(tenantId)
    {
        TenantMemberId = tenantMemberId;
        RoleId = roleId;
    }

    public Guid TenantMemberId { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; private set; }
}