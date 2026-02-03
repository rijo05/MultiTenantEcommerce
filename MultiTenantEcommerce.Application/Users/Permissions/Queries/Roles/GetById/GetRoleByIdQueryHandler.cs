using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Roles.GetById;
public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDetailResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly IPermissionRepository _permissionRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        RolesMapper rolesMapper)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<RoleDetailResponseDTO> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermisionsAsync(request.RoleId)
            ?? throw new Exception("Role not found");

        var permissions = await _permissionRepository.GetByIdsAsync(role.Permissions.Select(x => x.PermissionId));

        return _rolesMapper.ToRoleDetailResponseDTO(role, permissions);
    }
}
