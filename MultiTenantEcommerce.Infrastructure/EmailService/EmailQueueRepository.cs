using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
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
            job.FromName,
            job.ToEmail,
            job.ReplyToEmail ?? "",
            job.TemplateName,
            job.Priority,
            job.Metadata);
        await _appDbContext.EmailQueue.AddAsync(email);
    }

    public async Task<List<EmailJobDataDTO>> GetBatchUnprocessedEmailsAsync(EventPriority priority ,int batchSize)
    {
        using var transaction = await _appDbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

        var emails = await _appDbContext.EmailQueue
            .Where(x => x.Status == EmailStatus.Pending && x.AttemptCount < 5 && x.Priority == priority)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync();

        foreach (var email in emails)
        {
            email.Status = EmailStatus.Processing;
        }

        await _appDbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return ReturnBuilder(emails);
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

    private List<EmailJobDataDTO> ReturnBuilder(List<EmailQueue> emails)
    {
        return emails.Select(e => new EmailJobDataDTO(
            e.Id,
            e.TenantID,
            e.FromName,
            e.ToEmail,
            e.TemplateName,
            e.Priority,
            e.Metadata,
            e.ReplyToEmail
        )).ToList();
    }
}
