using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Platform.Identity.Auth.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.CreateMember;

public class CreateTenantMemberCommandHandler : ICommandHandler<CreateTenantMemberCommand, AuthTenantMemberResponseDTO>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly PasswordGenerator _passwordGenerator;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTenantMemberCommandHandler(ITenantMemberRepository employeeRepository,
        IPermissionRepository permissionRepository,
        ITenantContext tenantContext,
        TenantMemberMapper employeeMapper,
        IUnitOfWork unitOfWork,
        PasswordGenerator passwordGenerator,
        ITokenService tokenService,
        IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _permissionRepository = permissionRepository;
        _tenantContext = tenantContext;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
    }

    public async Task<AuthTenantMemberResponseDTO> Handle(CreateTenantMemberCommand request,
        CancellationToken cancellationToken)
    {
        var existingTenantMember = await _employeeRepository.GetByEmail(new Email(request.Email));
        if (existingTenantMember is not null)
            throw new Exception("TenantMember with this email already exists.");

        var roles = await _roleRepository.GetRolesWithPermissionsByIdsAsync(request.RolesId);

        if (roles.Count != request.RolesId.Count)
            throw new Exception("One or more roles do not exist.");

        var permissionIds = roles
            .SelectMany(r => r.Permissions)
            .Select(rp => rp.PermissionId)
            .Distinct()
            .ToList();

        var permissionNames = await _permissionRepository.GetPermissionNamesByIdsAsync(permissionIds);
        var roleNames = roles.Select(r => r.Name).ToList();

        var randomPassword = _passwordGenerator.GenerateRandomPassword();

        var employee = new TenantMember(
            _tenantContext.TenantId,
            request.Name,
            new Email(request.Email),
            new Password(randomPassword),
            request.RolesId);

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.CommitAsync();

        return new AuthTenantMemberResponseDTO
        {
            Id = employee.Id,
            Email = employee.Email.Value,
            Name = employee.Name,
            Permissions = permissionNames,
            Roles = roleNames,
            Token = _tokenService.GenerateToken(employee, roleNames, permissionNames)
        };
    }
}