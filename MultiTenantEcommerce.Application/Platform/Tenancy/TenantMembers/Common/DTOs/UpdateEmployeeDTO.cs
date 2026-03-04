namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

public record UpdateTenantMemberDTO(
    string? Name,
    string? Email,
    string? Password);