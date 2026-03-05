using MultiTenantEcommerce.Shared.Domain.Utilities.Guards;

namespace MultiTenantEcommerce.Shared.Domain.ValueObjects;

public class NonNegativeQuantity
{
    private NonNegativeQuantity()
    {
    }

    public NonNegativeQuantity(int value)
    {
        ValidateQuantity(value);
        Value = value;
    }

    public int Value { get; private set; }

    public static NonNegativeQuantity operator -(NonNegativeQuantity value1, NonNegativeQuantity value2)
    {
        var result = value1.Value - value2.Value;

        return new NonNegativeQuantity(result);
    }

    public static NonNegativeQuantity operator +(NonNegativeQuantity value1, NonNegativeQuantity value2)
    {
        var result = value1.Value + value2.Value;

        return new NonNegativeQuantity(result);
    }

    public void UpdateQuantity(int quantity)
    {
        ValidateQuantity(quantity);
        if (Value == quantity) throw new Exception("New quantity can't be equal to previous quantity");

        Value = quantity;
    }

    private void ValidateQuantity(int quantity)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));
    }
}