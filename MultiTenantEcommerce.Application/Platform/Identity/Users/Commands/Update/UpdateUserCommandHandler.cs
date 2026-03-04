using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Identity.Users.Commands.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, TenantMemberResponseDTO>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(ITenantMemberRepository employeeRepository,
        IRoleRepository roleRepository,
        TenantMemberMapper employeeMapper,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TenantMemberResponseDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id)
                       ?? throw new Exception("TenantMember doesnt exist.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _employeeRepository.GetByEmail(new Email(request.Email));
            if (existingEmail is not null && existingEmail.Id != employee.Id)
                throw new Exception("Email already in use.");
        }

        employee.UpdateTenantMember(
            request.Name,
            request.Email,
            request.Password,
            request.IsActive);

        var roleIds = employee.TenantMemberRoles.Select(x => x.RoleId).ToList();

        var roles = roleIds.Any()
            ? await _roleRepository.GetByIdsAsync(roleIds)
            : new List<Role>();

        await _unitOfWork.CommitAsync();
        return _employeeMapper.ToTenantMemberResponseDTO(employee, roles);
    }
}