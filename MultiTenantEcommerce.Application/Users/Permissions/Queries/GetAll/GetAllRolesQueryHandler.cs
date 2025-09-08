using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.GetAll;
public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, List<RoleResponseDTO>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

    public async Task<List<RoleResponseDTO>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllRolesAndPermissions(request.Page, 
            request.PageSize, 
            request.Sort);

        return _rolesMapper.ToRoleResponseDTOList(roles);
    }
}
