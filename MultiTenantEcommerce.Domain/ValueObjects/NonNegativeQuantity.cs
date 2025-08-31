using MultiTenantEcommerce.Domain.Common.Guard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class NonNegativeQuantity
{
    public int Value { get; private set; }

    private NonNegativeQuantity() { }

    public NonNegativeQuantity(int value)
    {
        ValidateQuantity(value);
        Value = value;
    }

    public static NonNegativeQuantity operator -(NonNegativeQuantity value1, NonNegativeQuantity value2)
    {
        var result = value1.Value - value2.Value;

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
