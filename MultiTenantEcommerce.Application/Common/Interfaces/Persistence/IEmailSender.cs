namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IEmailSender
{
    public Task SendAsync(string toEmail, string subject, string htmlBody, string textBody, string? replyToEmail = null);
}
