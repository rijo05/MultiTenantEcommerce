using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.DTOs.Address;
public record AddressValidationResult(
    bool IsValid,
    double Confidence,
    GeoLocation? Location,
    string FormattedAddress,
    Dictionary<string, string> Components);
