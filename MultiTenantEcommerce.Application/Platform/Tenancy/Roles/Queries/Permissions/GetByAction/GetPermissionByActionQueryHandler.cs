using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetByAction;

public class GetPermissionByActionQueryHandler : IQueryHandler<GetPermissionByActionQuery, List<PermissionResponseDTO>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;

    public GetPermissionByActionQueryHandler(IPermissionRepository permissionRepository,
        RolesMapper rolesMapper)
    {
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<List<PermissionResponseDTO>> Handle(GetPermissionByActionQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetByAction(request.Action);

        return _rolesMapper.ToPermissionList(permissions);
    }
}