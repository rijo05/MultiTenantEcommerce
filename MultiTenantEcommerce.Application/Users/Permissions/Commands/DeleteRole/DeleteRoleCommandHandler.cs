using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.DeleteRole;
public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Unit>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.roleId)
            ?? throw new Exception("Role doesnt exist.");

        if (await _employeeRepository.HasEmployeesWithRole(request.roleId))
            throw new Exception("There are employees with this role. Impossible to delete");

        await _roleRepository.DeleteAsync(role);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
