namespace MultiTenantEcommerce.Shared.Infrastructure.Services;

public interface IEmailSender
{
    public Task SendAsync(string toEmail, string fromName, string subject, string htmlBody,
        string? replyToEmail = null);
}