namespace MultiTenantEcommerce.Shared.Integration.Events.Commerce;
public record OrderCheckoutFailIntegrationEvent(Guid TenantId, IEnumerable<(Guid ProductId, int Quantity)> items) : IntegrationEvent(TenantId);
