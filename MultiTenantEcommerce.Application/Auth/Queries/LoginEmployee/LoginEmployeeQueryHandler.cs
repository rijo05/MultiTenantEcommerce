using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Queries.LoginEmployee;
public class LoginEmployeeQueryHandler : IQueryHandler<LoginEmployeeQuery, AuthEmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenService _tokenService;

    public LoginEmployeeQueryHandler(IEmployeeRepository employeeRepository,
        ITokenService tokenService)
    {
        _employeeRepository = employeeRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthEmployeeResponseDTO> Handle(LoginEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByEmailWithRolesAsync(new Email(request.Email));

        if (employee is null || !employee.Password.VerifySamePassword(request.Password))
            throw new Exception("Email or password are wrong");


        return new AuthEmployeeResponseDTO
        {
            Email = request.Email,
            Id = employee.Id,
            Name = employee.Name,
            Permissions = employee.EmployeeRoles.SelectMany(x => x.Role.Permissions.Select(x => x.Name)).ToList(),
            Roles = employee.EmployeeRoles.Select(x => x.Role.Name).ToList(),
            Token = _tokenService.CreateToken(employee)
        };
    }
}
