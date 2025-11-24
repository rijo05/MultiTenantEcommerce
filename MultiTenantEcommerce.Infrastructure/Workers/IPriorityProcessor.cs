using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.Workers;
public interface IPriorityProcessor
{
    Task ExecuteAsync(EventPriority priority);
}
