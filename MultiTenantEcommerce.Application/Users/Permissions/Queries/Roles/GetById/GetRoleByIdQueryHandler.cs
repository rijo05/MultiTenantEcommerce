using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Roles.GetById;
public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, RoleResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository, RolesMapper rolesMapper)
    {
        _roleRepository = roleRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<RoleResponseDTO> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAndPermissionById(request.roleId)
            ?? throw new Exception("Role not found");

        return _rolesMapper.ToRoleResponseDTO(role);
    }
}
