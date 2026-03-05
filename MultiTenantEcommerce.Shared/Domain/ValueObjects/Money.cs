using MultiTenantEcommerce.Shared.Domain.Utilities.Guards;

namespace MultiTenantEcommerce.Shared.Domain.ValueObjects;

public class Money
{
    private Money()
    {
    }

    public Money(decimal price)
    {
        ValidatePrice(price);

        Value = price;
    }

    public decimal Value { get; private set; }

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

    public long ToLong()
    {
        return long.Parse(Value.ToString());
    }
}