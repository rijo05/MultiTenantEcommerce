using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class PhoneNumber
{
    public string CountryCode { get; private set; }
    public string PhoneNumner {  get; private set; }

    public PhoneNumber(string countryCode, string phoneNumner)
    {
        ValidateCountryCode(countryCode);
        ValidatePhoneNumber(phoneNumner);

        CountryCode = countryCode;
        PhoneNumner = phoneNumner;
    }

    private void ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) throw new Exception("Phone number cannot be null or empty.");
    }

    public void ValidateCountryCode(string countryCode)
    {
        if (string.IsNullOrEmpty(countryCode)) throw new Exception("Country Code cannot be null or empty.");
    }

}
