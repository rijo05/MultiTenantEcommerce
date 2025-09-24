using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Users.Employees.EventHandlers.cs;
public class SendEmailOnEmployeeRegisteredEventHandler : IEventHandler<EmployeeRegisteredEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITenantRepository _tenantRepository;

    public SendEmailOnEmployeeRegisteredEventHandler(IEmailQueueRepository emailQueueRepository,
        IEmployeeRepository employeeRepository,
        ITenantRepository tenantRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _employeeRepository = employeeRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task HandleAsync(EmployeeRegisteredEvent domainEvent)
    {
        var employee = await _employeeRepository.GetByIdAsync(domainEvent.EmployeeId)
            ?? throw new Exception("Employee not found");

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = employee.Name,
            [EmailMetadataKeys.ChangePasswordLink] = "www.link.com", //############### TODO
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            employee.Email.Value,
            EmailTemplateNames.EmployeeRegistered,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}
