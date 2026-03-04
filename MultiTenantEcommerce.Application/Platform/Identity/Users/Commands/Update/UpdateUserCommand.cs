using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Identity.Users.Commands.Update;

public record UpdateUserCommand(
    Guid Id,
    string? Name,
    string? Email,
    string? Password,
    string? Role,
    bool? IsActive) : ICommand<TenantMemberResponseDTO>;