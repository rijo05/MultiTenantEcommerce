using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTenantEcommerce.Domain.Commerce.Customers.Events;
using MultiTenantEcommerce.Shared.Application.Auth;
using MultiTenantEcommerce.Shared.Application.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;

namespace MultiTenantEcommerce.Application.Commerce.Customers.EventHandlers;
public class CustomerRegisteredEventHandler : ISyncHandler<CustomerRegisteredEvent>
{
    private readonly IEmailVerificationTokenService _emailVerificationTokenService;
    private readonly IIntegrationEventPublisher _eventPublisher;

    public CustomerRegisteredEventHandler(IEmailVerificationTokenService emailVerificationTokenService, 
        IIntegrationEventPublisher eventPublisher)
    {
        _emailVerificationTokenService = emailVerificationTokenService;
        _eventPublisher = eventPublisher;
    }

    public Task HandleAsync(CustomerRegisteredEvent evt)
    {
        var token = _emailVerificationTokenService.GenerateToken(evt.CustomerId, TimeSpan.FromDays(1));

        _eventPublisher.AddEvent(new CustomerVerificationRequestedIntegrationEvent(evt.TenantId, evt.CustomerId, evt.CustomerEmail, evt.CustomerName, token));

        _eventPublisher.AddEvent(new CustomerRegisteredIntegrationEvent(evt.TenantId, evt.CustomerId, evt.CustomerEmail, evt.CustomerName));

        return Task.CompletedTask;
    }
}
