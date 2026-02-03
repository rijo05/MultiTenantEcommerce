using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Mappers;
public class RolesMapper
{
    public RoleResponseDTO ToRoleResponseDTO(Role role)
    {
        return new RoleResponseDTO
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };
    }
    public List<RoleResponseDTO> ToRoleResponseDTOList(List<Role> roles)
    {
        return roles.Select(x => ToRoleResponseDTO(x)).ToList();
    }

    public RoleDetailResponseDTO ToRoleDetailResponseDTO(Role role, List<Permission> permissions)
    {
        return new RoleDetailResponseDTO
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt,

            Permissions = ToPermissionList(permissions)
        };
    }

    public PermissionResponseDTO ToPermissionResponseDTO(Permission p)
    {
        return new PermissionResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            Area = p.Area,
            Action = p.Action,
            Description = p.Description
        };
    }

    public List<PermissionResponseDTO> ToPermissionList(List<Permission> permissions)
    {
        return permissions.Select(x => ToPermissionResponseDTO(x)).ToList();
    }

}
