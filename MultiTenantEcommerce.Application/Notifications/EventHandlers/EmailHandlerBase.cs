using MultiTenantEcommerce.Domain.Notifications.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Events;
using MultiTenantEcommerce.Shared.Utilities.Constants;
using Polly;
using Polly.Retry;

namespace MultiTenantEcommerce.Application.Notifications.EventHandlers;

public abstract class EmailHandlerBase<TEvent> : IAsyncHandler<TEvent>
    where TEvent : IIntegrationEvent
{
    private readonly ITenantNotificationProfileRepository _profileRepository;
    private readonly ITemplateRender _renderer;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IEmailSender _sender;

    protected EmailHandlerBase(
        IEmailSender sender,
        ITemplateRender renderer,
        ITenantNotificationProfileRepository profileRepository)
    {
        _sender = sender;
        _renderer = renderer;
        _profileRepository = profileRepository;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));
    }

    public async Task HandleAsync(TEvent domainEvent)
    {
        var packet = await GetEmailDataAsync(domainEvent);

        var profile = await _profileRepository.GetByTenantIdAsync(domainEvent.TenantId) ?? criarDefault();

        var overrideSettings = profile.Overrides.FirstOrDefault(o => o.TemplateName == packet.TemplateName);

        var htmlTemplatePath = Path.Combine(AppContext.BaseDirectory, "Templates", $"{packet.TemplateName}.html");
        var htmlTemplate = await File.ReadAllTextAsync(htmlTemplatePath);

        var scribanModel = new
        {
            profile.Theme,
            Override = overrideSettings,
            Data = packet.DynamicData
        };

        var htmlBody = _renderer.Render(htmlTemplate, scribanModel);

        var rawSubject = overrideSettings?.CustomSubject ?? GetSystemDefaultSubject(packet.TemplateName);
        var finalSubject = _renderer.Render(rawSubject, scribanModel);


        await _retryPolicy.ExecuteAsync(() =>
            _sender.SendAsync(packet.ToEmail, packet.FromName, finalSubject, htmlBody, packet.ReplyToEmail));
    }

    protected abstract Task<EmailDataPacket> GetEmailDataAsync(TEvent evt);

    private static string GetSystemDefaultSubject(EmailTemplateNames templateName)
    {
        return templateName switch
        {
            EmailTemplateNames.OrderPaid => "A sua encomenda {{Data.OrderNumber}} foi paga com sucesso",
            EmailTemplateNames.CustomerRegistered => "Bem-vindo à nossa plataforma!",
            EmailTemplateNames.ShipmentShipped => "A sua encomenda já foi expedida",
            _ => "Nova notificação da loja"
        };
    }
}