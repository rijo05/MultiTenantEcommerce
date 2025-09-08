using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class PositiveQuantity
{
    public int Value { get; private set; }

    private PositiveQuantity() { }

    public PositiveQuantity(int value)
    {
        ValidateQuantity(value);
        Value = value;
    }

    public static PositiveQuantity operator -(PositiveQuantity value1, PositiveQuantity value2)
    {
        var result = value1.Value - value2.Value;

        return new PositiveQuantity(result);
    }

    public void UpdateQuantity(int quantity)
    {
        ValidateQuantity(quantity);
        if (Value == quantity) throw new Exception("New quantity can't be equal to previous quantity");

        Value = quantity;
    }

    public long ToLong()
    {
        return long.Parse(Value.ToString());
    }

    private void ValidateQuantity(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
    }


}

