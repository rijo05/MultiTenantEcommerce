using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public class EmailSender : IEmailSender
{
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _appPassword;

    public EmailSender(IConfiguration config)
    {
        _fromEmail = config["Email:Gmail:FromEmail"];
        _appPassword = config["Email:Gmail:AppPassword"];
    }

    public async Task SendAsync(string toEmail, string fromName, string subject, string htmlBody, string textBody, string? replyToEmail = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, _fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        if (!string.IsNullOrEmpty(replyToEmail))
            message.ReplyTo.Add(MailboxAddress.Parse(replyToEmail));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_fromEmail, _appPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
