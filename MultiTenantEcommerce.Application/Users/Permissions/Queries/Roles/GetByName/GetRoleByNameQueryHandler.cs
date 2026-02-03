using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Roles.GetByName;
public class GetRoleByNameQueryHandler : IQueryHandler<GetRoleByNameQuery, RoleDetailResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
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
