namespace MultiTenantEcommerce.Shared.Integration.Events.Commerce;

public record ProductCreatedIntegrationEvent(
    Guid TenantId,
    Guid ProductId) : IntegrationEvent(TenantId);
