using MultiTenantEcommerce.Application.Platform.Identity.Auth.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.CreateMember;

public record CreateTenantMemberCommand(
    string Name,
    string Email,
    List<Guid> RolesId) : ICommand<AuthTenantMemberResponseDTO>;