using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IEmailQueueRepository
{
    Task EnqueueEmailAsync(EmailJobDataDTO emailJob);
    Task<List<EmailJobDataDTO>> GetBatchUnprocessedEmailsAsync(EventPriority priority, int batchSize);
    Task MarkEmailAsSentAsync(Guid emailId);
    Task MarkEmailAsFailedAsync(Guid emailId, string errorMessage);
}
