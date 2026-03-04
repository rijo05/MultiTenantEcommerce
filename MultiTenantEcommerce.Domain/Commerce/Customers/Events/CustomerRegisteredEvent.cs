using MultiTenantEcommerce.Shared.Domain;
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;

namespace MultiTenantEcommerce.Domain.Commerce.Customers.Events;

public record CustomerRegisteredEvent(
    Guid TenantId, 
    Guid CustomerId,
    string CustomerEmail,
    string CustomerName) : DomainEvent(TenantId);