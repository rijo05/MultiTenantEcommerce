namespace MultiTenantEcommerce.Shared.Application.Interfaces;

public interface IEmailSender
{
    public Task SendAsync(string toEmail, string fromName, string subject, string htmlBody,
        string? replyToEmail = null);
}