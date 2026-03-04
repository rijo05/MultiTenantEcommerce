using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.AddToRole;

public class AddPermissionsToRoleCommandHandler : ICommandHandler<AddPermissionsToRoleCommand, RoleDetailResponseDTO>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
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

    public async Task<RoleDetailResponseDTO> Handle(AddPermissionsToRoleCommand request,
        CancellationToken cancellationToken)
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

        foreach (var id in newIds) role.AddPermission(id);

        await _unitOfWork.CommitAsync();

        return _rolesMapper.ToRoleDetailResponseDTO(role, allPermissions);
    }
}