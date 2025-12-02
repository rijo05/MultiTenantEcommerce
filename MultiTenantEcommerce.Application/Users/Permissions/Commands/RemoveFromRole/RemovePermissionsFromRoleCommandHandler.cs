using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.RemoveFromRole;
public class RemovePermissionsFromRoleCommandHandler : ICommandHandler<RemovePermissionsFromRoleCommand, RoleDetailResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
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

    public async Task<RoleDetailResponseDTO> Handle(RemovePermissionsFromRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermisionsAsync(request.RoleId)
            ?? throw new Exception("Role doesnt exist.");

        var idsToRemove = request.Permissions.Distinct();

        foreach (var pId in idsToRemove)
        {
            role.RemovePermission(pId);
        }

        await _unitOfWork.CommitAsync();

        var remainingIds = role.Permissions.Select(x => x.PermissionId).ToList();

        var remainingPermissions = remainingIds.Any()
            ? await _permissionRepository.GetByIdsAsync(remainingIds)
            : new List<Permission>();
            
        return _rolesMapper.ToRoleDetailResponseDTO(role, remainingPermissions);
    }
}
