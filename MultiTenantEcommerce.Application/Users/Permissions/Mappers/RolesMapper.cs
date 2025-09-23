using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Mappers;
public class RolesMapper
{
    public RoleResponseDTO ToRoleResponseDTO(Role role)
    {
        return new RoleResponseDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            Permissions = ToPermissionResponseDTOList(role.Permissions),
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,
        };
    }

    public List<RoleResponseDTO> ToRoleResponseDTOList(IEnumerable<Role> roles)
    {
        return roles.Select(x => ToRoleResponseDTO(x)).ToList();
    }

    public PermissionResponseDTO ToPermissionResponseDTO(Permission permission)
    {
        return new PermissionResponseDTO()
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            Action = permission.Action,
            Area = permission.Area,
        };
    }

    public List<PermissionResponseDTO> ToPermissionResponseDTOList(IEnumerable<Permission> permissions)
    {
        return permissions.Select(x => ToPermissionResponseDTO(x)).ToList();
    }
}
