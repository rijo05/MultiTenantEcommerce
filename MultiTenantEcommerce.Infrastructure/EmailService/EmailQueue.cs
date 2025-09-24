using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public class EmailQueue
{
    public Guid Id { get; private set; }
    public Guid TenantID { get; private set; }
    public string ToEmail { get; private set; }
    public string FromEmail { get; private set; }
    public string ReplyToEmail { get; set; }
    public string? Subject { get; private set; }
    public string? Body { get; private set; }
    public EmailTemplateNames TemplateName { get; private set; }
    public Dictionary<string, string> Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? SentAt { get; private set; }
    public EmailStatus Status { get; set; }
    public string? ErrorMessage { get; private set; }
    public int AttemptCount { get; private set; } = 0;

    private EmailQueue() { }
    public EmailQueue(Guid tenantId, string to, string replyTo, EmailTemplateNames templateName, Dictionary<string, string> metadata)
    {
        Id = Guid.NewGuid();
        TenantID = tenantId;
        ToEmail = to;
        ReplyToEmail = replyTo;
        FromEmail = "myemail@gmail.com";
        TemplateName = templateName;
        CreatedAt = DateTime.UtcNow;
        Status = EmailStatus.Pending;
        Metadata = metadata ?? new Dictionary<string, string>();

        Subject = "";
        Body = "";
    }

    internal void SetSentAt()
    {
        SentAt = DateTime.UtcNow;
        Status = EmailStatus.Sent;
    }

    internal void SetErrors(string error)
    {
        ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? error : $"{ErrorMessage}\n{error}";
        Status = EmailStatus.Failed;
    }

    internal void IncrementAttempts() => AttemptCount++;

}
