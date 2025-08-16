namespace MultiTenantEcommerce.Domain.ValueObjects;
public class PhoneNumber
{
    public string CountryCode { get; private set; }
    public string Number {  get; private set; }

    private PhoneNumber() { }
    public PhoneNumber(string countryCode, string Number)
    {
        ValidateCountryCode(countryCode);
        ValidatePhoneNumber(Number);

        CountryCode = countryCode.Replace(" ", "");
        Number = Number.Replace(" ", "");
    }

    private void ValidatePhoneNumber(string Number)
    {
        if (string.IsNullOrEmpty(Number)) throw new Exception("Phone number cannot be null or empty.");
    }

    public void ValidateCountryCode(string countryCode)
    {
        if (string.IsNullOrEmpty(countryCode)) throw new Exception("Country Code cannot be null or empty.");
    }

    public override string ToString()
    {
        return $"+{CountryCode} {Number}";
    }

}
