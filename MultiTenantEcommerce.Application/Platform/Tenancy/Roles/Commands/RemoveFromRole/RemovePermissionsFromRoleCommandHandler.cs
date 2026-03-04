using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.RemoveFromRole;

public class
    RemovePermissionsFromRoleCommandHandler : ICommandHandler<RemovePermissionsFromRoleCommand, RoleDetailResponseDTO>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePermissionsFromRoleCommandHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        RolesMapper rolesMapper,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDetailResponseDTO> Handle(RemovePermissionsFromRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermisionsAsync(request.RoleId)
                   ?? throw new Exception("Role doesnt exist.");

        var idsToRemove = request.Permissions.Distinct();

        foreach (var pId in idsToRemove) role.RemovePermission(pId);

        await _unitOfWork.CommitAsync();

        var remainingIds = role.Permissions.Select(x => x.PermissionId).ToList();

        var remainingPermissions = remainingIds.Any()
            ? await _permissionRepository.GetByIdsAsync(remainingIds)
            : new List<Permission>();

        return _rolesMapper.ToRoleDetailResponseDTO(role, remainingPermissions);
    }
}