using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

public class TenantMemberResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public List<RoleResponseDTO> Roles { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool IsActive { get; init; }
}