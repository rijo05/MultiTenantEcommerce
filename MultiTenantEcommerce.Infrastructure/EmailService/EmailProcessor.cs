using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Templates.Entities;
using Polly;
using Polly.Retry;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public class EmailProcessor
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IServiceProvider _sp;
    private readonly AsyncRetryPolicy _retryPolicy;
    private const int BATCH_SIZE = 20;

    public EmailProcessor(IEmailQueueRepository emailQueueRepository,
        IServiceProvider sp,
        IEmailTemplateRepository emailTemplateRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _sp = sp;
        _emailTemplateRepository = emailTemplateRepository;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (ex, ts, attempt, ctx) =>
                Console.WriteLine((ex, "Retry {Attempt} in {Delay}s", attempt, ts.TotalSeconds)));
    }

    public async Task ExecuteAsync()
    {
        Console.WriteLine("entrei nos emails 1");
        var emails = await _emailQueueRepository.GetBatchUnprocessedEmailsAsync(BATCH_SIZE);
        Console.WriteLine("entrei nos emails 2");

        using var scope = _sp.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var renderer = scope.ServiceProvider.GetRequiredService<TemplateRenderer>();

        foreach (var item in emails)
        {
            Console.WriteLine("entrei nos emails 3");
            try
            {
                var template = await _emailTemplateRepository.GetDefaultTemplateByTemplateName(item.TemplateName);

                var html = renderer.Render(template.HtmlContent, item.Metadata);

                var renderedSubject = renderer.Render(template.Subject, item.Metadata);

                var text = string.IsNullOrWhiteSpace(template.TextContent)
                    ? HtmlToText.Convert(html)
                    : renderer.Render(template.TextContent, item.Metadata);


                await _retryPolicy.ExecuteAsync(() =>
                    sender.SendAsync(item.ToEmail, renderedSubject, html, text, item.ReplyToEmail)
                );

                await _emailQueueRepository.MarkEmailAsSentAsync(item.Id);
            }
            catch (Exception ex)
            {
                await _emailQueueRepository.MarkEmailAsFailedAsync(item.Id, ex.Message);
            }

            await Task.Delay(50);
        }
    }
}
