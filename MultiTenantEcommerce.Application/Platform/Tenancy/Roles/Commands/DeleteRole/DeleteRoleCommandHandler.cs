using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Unit>
{
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository, ITenantMemberRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.RoleId)
                   ?? throw new Exception("Role doesnt exist.");

        role.CanItBeModifiedOrDeleted();

        if (await _employeeRepository.HasTenantMembersWithRole(request.RoleId))
            throw new Exception("There are employees with this role. Impossible to delete");

        await _roleRepository.DeleteAsync(role);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}