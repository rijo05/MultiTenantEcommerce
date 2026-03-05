using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Shared.EmailService;

public class EmailSender : IEmailSender
{
    private readonly string _appPassword;
    private readonly string _fromEmail;

    public EmailSender(IConfiguration config)
    {
        _fromEmail = config["Email:Gmail:FromEmail"];
        _appPassword = config["Email:Gmail:AppPassword"];
    }

    public async Task SendAsync(string toEmail, string fromName, string subject, string htmlBody,
        string? replyToEmail = null)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, _fromEmail));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        if (!string.IsNullOrEmpty(replyToEmail))
            message.ReplyTo.Add(MailboxAddress.Parse(replyToEmail));

        var textBody = HtmlToText.Convert(htmlBody);

        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_fromEmail, _appPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}