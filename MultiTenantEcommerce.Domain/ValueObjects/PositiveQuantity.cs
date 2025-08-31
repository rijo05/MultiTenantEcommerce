using MultiTenantEcommerce.Domain.Common.Guard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    private void ValidateQuantity(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
    }
}

