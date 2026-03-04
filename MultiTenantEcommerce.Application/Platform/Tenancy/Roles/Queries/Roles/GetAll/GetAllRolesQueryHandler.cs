using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetAll;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, List<RoleResponseDTO>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository, RolesMapper rolesMapper)
    {
        _roleRepository = roleRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<List<RoleResponseDTO>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllRolesAndPermissions(request.Page,
            request.PageSize,
            request.Sort);

        return _rolesMapper.ToRoleResponseDTOList(roles);
    }
}