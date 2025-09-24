using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public class EmailQueueRepository : IEmailQueueRepository
{
    private readonly AppDbContext _appDbContext;

    public EmailQueueRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task EnqueueEmailAsync(EmailJobDataDTO job)
    {
        var email = new EmailQueue(
            job.TenantId,
            job.ToEmail,
            job.ReplyToEmail ?? "",
            job.TemplateName,
            job.Metadata);
        await _appDbContext.EmailQueue.AddAsync(email);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<List<EmailJobDataDTO>> GetBatchUnprocessedEmailsAsync(int batchSize)
    {
        var entities = await _appDbContext.EmailQueue
            .Where(e => e.Status == EmailStatus.Pending && e.AttemptCount < 5)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync();

        return entities.Select(e => new EmailJobDataDTO(
            e.Id,
            e.TenantID,
            e.ToEmail,
            e.TemplateName,
            e.Metadata,
            e.ReplyToEmail
        )).ToList();
    }

    public async Task MarkEmailAsSentAsync(Guid emailId)
    {
        var entity = await _appDbContext.EmailQueue.FindAsync(emailId);
        if (entity == null) return;
        entity.SetSentAt();
        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkEmailAsFailedAsync(Guid emailId, string errorMessage)
    {
        var entity = await _appDbContext.EmailQueue.FindAsync(emailId);
        if (entity == null) return;
        entity.SetErrors(errorMessage);
        entity.IncrementAttempts();
        await _appDbContext.SaveChangesAsync();
    }
}
