using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
public class CustomerAddress : BaseEntity
{
    public Guid Id { get; private set; } 
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    public string HouseNumber { get; private set; }

    private CustomerAddress() { }

    internal CustomerAddress(string street, string city, string postalCode, string country, string houseNumber)
    {
        ValidateAddress(street, city, postalCode, country, houseNumber);

        Id = Guid.NewGuid();
        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
        HouseNumber = houseNumber;
    }

    private void ValidateAddress(string street,
    string city,
    string postalCode,
    string country,
    string houseNumber)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("PostalCode cannot be empty.", nameof(postalCode));

        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty.", nameof(country));

        if (string.IsNullOrWhiteSpace(houseNumber))
            throw new ArgumentException("HouseNumber cannot be empty.", nameof(houseNumber));
    }
}
