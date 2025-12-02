using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetByArea;
public class GetPermissionByAreaQueryHandler : IQueryHandler<GetPermissionByAreaQuery, List<PermissionResponseDTO>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;

    public GetPermissionByAreaQueryHandler(IPermissionRepository permissionRepository,
        RolesMapper rolesMapper)
    {
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<List<PermissionResponseDTO>> Handle(GetPermissionByAreaQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetByArea(request.Area);

        return _rolesMapper.ToPermissionList(permissions);
    }
}
