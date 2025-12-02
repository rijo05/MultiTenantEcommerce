using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.EventHandlers.cs;
public class SendEmailOnEmployeeRegisteredEventHandler : IEventHandler<EmployeeRegisteredEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITenantRepository _tenantRepository;

    public SendEmailOnEmployeeRegisteredEventHandler(IEmailQueueRepository emailQueueRepository,
        IRoleRepository roleRepository,
        IEmployeeRepository employeeRepository,
        ITenantRepository tenantRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task HandleAsync(EmployeeRegisteredEvent domainEvent)
    {
        var employee = await _employeeRepository.GetByIdAsync(domainEvent.EmployeeId)
            ?? throw new Exception("Employee not found");

        var roles = await _roleRepository.GetByIdsAsync(employee.EmployeeRoles.Select(x => x.RoleId).ToList());

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.EmployeeName] = employee.Name,
            [EmailMetadataKeys.ChangePasswordLink] = "www.link.com", //############### TODO
            [EmailMetadataKeys.RolesHtml] = $"<ul>{string.Join("", roles.Select(er => $"<li>{er.Name}</li>"))}</ul>",
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            tenant.Name,
            employee.Email.Value,
            EmailTemplateNames.EmployeeRegistered,
            domainEvent.EventPriority,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}
