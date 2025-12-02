using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.AddToRole;
public class AddPermissionsToRoleCommandHandler : ICommandHandler<AddPermissionsToRoleCommand, RoleDetailResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddPermissionsToRoleCommandHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        RolesMapper rolesMapper,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDetailResponseDTO> Handle(AddPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdWithPermisionsAsync(request.RoleId)
            ?? throw new Exception("Role doesnt exist.");

        var existingIds = role.Permissions.Select(x => x.PermissionId);
        var newIds = request.Permissions.Distinct();

        var allIdsToFetch = existingIds.Union(newIds).ToList();

        var allPermissions = await _permissionRepository.GetByIdsAsync(allIdsToFetch);

        var fetchedIds = allPermissions.Select(p => p.Id).ToHashSet();
        var missingIds = newIds.Where(id => !fetchedIds.Contains(id)).ToList();

        if (missingIds.Any())
            throw new Exception($"Invalid permission ids: {string.Join(",", missingIds)}");

        foreach (var id in newIds)
        {
            role.AddPermission(id);
        }

        await _unitOfWork.CommitAsync();

        return _rolesMapper.ToRoleDetailResponseDTO(role, allPermissions);
    }
}
