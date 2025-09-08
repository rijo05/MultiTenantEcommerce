using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
public class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand, AuthTenantResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenService _tokenService;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        IEmployeeRepository employeeRepository,
        ITokenService tokenService)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _employeeRepository = employeeRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantRepository.GetByCompanyName(request.CompanyName);

        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var tenant = new Domain.Tenants.Entities.Tenant(request.CompanyName);

        var employee = new Employee(
            tenant.Id,
            request.OwnerName,
            new Email(request.OwnerEmail),
            new Password(request.Password));

        await _tenantRepository.AddAsync(tenant);
        await _employeeRepository.AddAsync(employee);

        await _unitOfWork.CommitAsync();

        return new AuthTenantResponse
        {
            Email = employee.Email.Value,
            Id = employee.Id,
            Name = employee.Name,
            Token = _tokenService.CreateToken(employee)
        };

        //por agora q ira haver apenas 1 plano gratis, no futuro vejo como fazer planos pagos
    }
}
