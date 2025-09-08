using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.CreateRole;
public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, RoleResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        RolesMapper rolesMapper,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleResponseDTO> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleRepository.GetByNameAsync(request.Name) is not null)
            throw new Exception("Role with that name already exists");

        var permissions = await _permissionRepository.GetByIdsAsync(request.permissions);

        if (permissions.Count != request.permissions.Distinct().Count())
            throw new Exception("Some ids are not valid");

        var role = new Role(request.Name, request.Description);

        foreach (var item in permissions)
        {
            role.AddPermission(item);
        }

        await _roleRepository.AddAsync(role);

        await _unitOfWork.CommitAsync();

        return _rolesMapper.ToRoleResponseDTO(role);
    }
}
