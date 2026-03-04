using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetByName;

public class GetRoleByNameQueryHandler : IQueryHandler<GetRoleByNameQuery, RoleDetailResponseDTO>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

    public GetRoleByNameQueryHandler(IRoleRepository roleRepository,
        RolesMapper rolesMapper,
        IPermissionRepository permissionRepository)
    {
        _roleRepository = roleRepository;
        _rolesMapper = rolesMapper;
        _permissionRepository = permissionRepository;
    }

    public async Task<RoleDetailResponseDTO> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByNameAsync(request.Name)
                   ?? throw new Exception("Role with that name doesnt exist");

        var permissions = await _permissionRepository.GetByIdsAsync(role.Permissions.Select(x => x.PermissionId));

        return _rolesMapper.ToRoleDetailResponseDTO(role, permissions);
    }
}