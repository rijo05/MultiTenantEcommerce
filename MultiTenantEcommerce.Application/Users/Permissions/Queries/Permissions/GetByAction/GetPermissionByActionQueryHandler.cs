using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetByAction;
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

    public async Task<List<PermissionResponseDTO>> Handle(GetPermissionByActionQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetByAction(request.Action);

        return _rolesMapper.ToPermissionResponseDTOList(permissions);
    }
}
