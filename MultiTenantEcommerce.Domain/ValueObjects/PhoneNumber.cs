namespace MultiTenantEcommerce.Domain.ValueObjects;
public class PhoneNumber
{
    public string CountryCode { get; private set; }
    public string Number { get; private set; }

    private PhoneNumber() { }
    public PhoneNumber(string countryCode, string number)
    {
        ValidateCountryCode(countryCode);
        ValidatePhoneNumber(number);

        CountryCode = countryCode.Replace(" ", "");
        Number = number.Replace(" ", "");
    }

    private void ValidatePhoneNumber(string number)
    {
        if (string.IsNullOrEmpty(number)) throw new Exception("Phone number cannot be null or empty.");
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
