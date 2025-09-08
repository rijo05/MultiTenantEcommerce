using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Money
{
    public decimal Value { get; private set; }

    private Money() { }
    public Money(decimal price)
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

    public long ToLong()
    {
        return long.Parse(Value.ToString());
    }
}
