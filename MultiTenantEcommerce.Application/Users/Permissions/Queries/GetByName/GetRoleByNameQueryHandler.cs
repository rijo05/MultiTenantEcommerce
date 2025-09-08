using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.GetByName;
public class GetRoleByNameQueryHandler : IQueryHandler<GetRoleByNameQuery, RoleResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;

    public GetRoleByNameQueryHandler(IRoleRepository roleRepository, RolesMapper rolesMapper)
    {
        _roleRepository = roleRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<RoleResponseDTO> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByNameAsync(request.Name)
            ?? throw new Exception("Role with that name doesnt exist");

        return _rolesMapper.ToRoleResponseDTO(role);
    }
}
