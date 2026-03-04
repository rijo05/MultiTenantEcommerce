using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.InviteMember;
public record InviteMemberCommand(string Email, List<Guid> RolesId) : ICommand<Unit>;
