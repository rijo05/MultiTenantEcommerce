using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class NIF
{
    public string Value { get; private set; }

    private NIF() { }

    public NIF(string value)
    {

    }

    private void ValidateNIF(string value)
    {
        GuardCommon.AgainstNullOrEmpty(value, nameof(value));
        Value = value;
    }
}
