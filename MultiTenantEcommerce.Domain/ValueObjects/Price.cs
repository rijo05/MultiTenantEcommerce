using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Price
{
    public decimal Value { get; private set; }

    public Price(decimal value)
    {
        GuardCommon.AgainstNegativeOrZero(value, nameof(value));

        Value = value;
    }
}
