using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetAll;
public class GetAllPermissionsQueryHandler : IQueryHandler<GetAllPermissionsQuery, List<PermissionResponseDTO>>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;

    public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository,
        RolesMapper rolesMapper)
    {
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
    }

    public async Task<List<PermissionResponseDTO>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAllAsync();

        return _rolesMapper.ToPermissionResponseDTOList(permissions);
    }
}
