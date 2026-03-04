using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;

public class TenantMemberMapper
{
    private readonly HateoasLinkService _hateoasLinkService;
    private readonly RolesMapper _rolesMapper;

    public TenantMemberMapper(HateoasLinkService hateoasLinkService,
        RolesMapper rolesMapper)
    {
        _hateoasLinkService = hateoasLinkService;
        _rolesMapper = rolesMapper;
    }

    public TenantMemberResponseDTO ToTenantMemberResponseDTO(TenantMember employee, List<Role> roles)
    {
        return new TenantMemberResponseDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email.Value,
            Roles = _rolesMapper.ToRoleResponseDTOList(roles),
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }

    public List<TenantMemberResponseDTO> ToTenantMemberResponseDTOList(List<TenantMember> employees,
        List<Role> allRoles)
    {
        var roleDict = allRoles.ToDictionary(r => r.Id);

        return employees.Select(emp =>
        {
            var empRoleIds = emp.TenantMemberRoles.Select(er => er.RoleId).ToList();

            var empRoles = empRoleIds
                .Select(id => roleDict.TryGetValue(id, out var r) ? r : null)
                .Where(r => r != null)
                .ToList()!;

            return ToTenantMemberResponseDTO(emp, empRoles);
        }).ToList();
    }
}