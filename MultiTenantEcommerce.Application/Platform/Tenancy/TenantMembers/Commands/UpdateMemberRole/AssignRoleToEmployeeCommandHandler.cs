using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.UpdateMemberRole;

public class
    AssignRoleToTenantMemberCommandHandler : ICommandHandler<AssignRoleToTenantMemberCommand, TenantMemberResponseDTO>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleToTenantMemberCommandHandler(ITenantMemberRepository employeeRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        TenantMemberMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _employeeMapper = employeeMapper;
    }

    public async Task<TenantMemberResponseDTO> Handle(AssignRoleToTenantMemberCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.employeeId)
                       ?? throw new Exception("TenantMember doesnt exist.");

        var roles = await _roleRepository.GetByIdsAsync(request.roles.Distinct());

        var missingIds = request.roles.Distinct().Except(roles.Select(r => r.Id)).ToList();
        if (missingIds.Any())
            throw new Exception($"Invalid role ids: {string.Join(", ", missingIds)}");

        foreach (var item in roles)
            employee.AddRole(item.Id);

        await _unitOfWork.CommitAsync();

        return _employeeMapper.ToTenantMemberResponseDTO(employee, roles);
    }
}