using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Price
{
    public decimal Value { get; private set; }

    public Price(decimal price)
    {
        ValidatePrice(price);

        Value = price;
    }

    public void UpdatePrice(decimal price)
    {
        ValidatePrice(price);
        if (Value == price) throw new Exception("New price can't be equal to previous price.");

        Value = price;
    }

    public void ValidatePrice(decimal price)
    {
        GuardCommon.AgainstNegativeOrZero(price, nameof(price));
    }
}
