using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.UpdateMemberRole;

public record RemoveRoleFromTenantMemberCommand(
    Guid employeeId,
    List<Guid> roles) : ICommand<TenantMemberResponseDTO>;