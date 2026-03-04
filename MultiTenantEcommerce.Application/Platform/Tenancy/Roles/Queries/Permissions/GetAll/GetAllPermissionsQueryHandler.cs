using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetAll;

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

    public async Task<List<PermissionResponseDTO>> Handle(GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAllAsync();

        return _rolesMapper.ToPermissionList(permissions);
    }
}