namespace MultiTenantEcommerce.Application.Shipping.DTOs;
public record ShipmentResultDTO(
    string TrackingNumber,
    string TrackingLink,
    byte[] LabelPdf,
    decimal price,
    TimeSpan MinTransit,
    TimeSpan MaxTransit);
