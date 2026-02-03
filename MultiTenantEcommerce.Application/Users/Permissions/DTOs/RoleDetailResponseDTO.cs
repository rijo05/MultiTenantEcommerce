namespace MultiTenantEcommerce.Application.Users.Permissions.DTOs;
public class RoleDetailResponseDTO : RoleResponseDTO
{
    public List<PermissionResponseDTO> Permissions { get; init; }
}
