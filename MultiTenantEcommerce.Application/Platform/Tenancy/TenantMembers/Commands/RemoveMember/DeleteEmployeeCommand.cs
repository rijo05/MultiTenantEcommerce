using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.RemoveMember;

public record DeleteTenantMemberCommand(
    Guid id) : ICommand<Unit>;