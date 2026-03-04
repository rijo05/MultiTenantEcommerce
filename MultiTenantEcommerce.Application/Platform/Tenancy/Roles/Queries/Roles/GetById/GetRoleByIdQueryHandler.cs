using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleDetailResponseDTO>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

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