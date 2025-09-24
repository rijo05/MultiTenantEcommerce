using MultiTenantEcommerce.Application.Common.DTOs;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IEmailQueueRepository
{
    Task EnqueueEmailAsync(EmailJobDataDTO emailJob);
    Task<List<EmailJobDataDTO>> GetBatchUnprocessedEmailsAsync(int batchSize);
    Task MarkEmailAsSentAsync(Guid emailId);
    Task MarkEmailAsFailedAsync(Guid emailId, string errorMessage);
}
