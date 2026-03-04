using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetByArea;

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

    public async Task<List<PermissionResponseDTO>> Handle(GetPermissionByAreaQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetByArea(request.Area);

        return _rolesMapper.ToPermissionList(permissions);
    }
}