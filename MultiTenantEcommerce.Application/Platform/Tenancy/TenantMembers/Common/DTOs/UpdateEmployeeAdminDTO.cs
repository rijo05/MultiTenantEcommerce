namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

public record UpdateTenantMemberAdminDTO(
    string? Name,
    string? Email,
    string? Password,
    string? Role,
    bool? IsActive);