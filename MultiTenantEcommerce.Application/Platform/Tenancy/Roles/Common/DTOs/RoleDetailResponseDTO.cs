namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

public class RoleDetailResponseDTO : RoleResponseDTO
{
    public List<PermissionResponseDTO> Permissions { get; init; }
}