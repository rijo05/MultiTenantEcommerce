using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, AuthEmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITenantContext _tenantContext;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordGenerator _passwordGenerator;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        ITenantContext tenantContext,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork,
        PasswordGenerator passwordGenerator,
        ITokenService tokenService,
        IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _tenantContext = tenantContext;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
    }

    public async Task<AuthEmployeeResponseDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _employeeRepository.GetByEmailAsync(new Email(request.Email));
        if (existingEmployee is not null)
            throw new Exception("Employee with this email already exists.");

        var roles = await _roleRepository.GetByIdsAsync(request.RolesId);

        var randomPassword = _passwordGenerator.GenerateRandomPassword();

        var employee = new Employee(
            _tenantContext.TenantId,
            request.Name,
            new Email(request.Email),
            new Password(randomPassword),
            roles);

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.CommitAsync();

        return new AuthEmployeeResponseDTO
        {
            Id = employee.Id,
            Email = employee.Email.Value,
            Name = employee.Name,
            Permissions = employee.EmployeeRoles.SelectMany(x => x.Role.Permissions.Select(x => x.Name)).ToList(),
            Roles = employee.EmployeeRoles.Select(x => x.Role.Name).ToList(),
            Token = _tokenService.CreateToken(employee)
        };
    }
}
