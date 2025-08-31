using System.Diagnostics.Metrics;
using System.IO;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    public string HouseNumber { get; private set; }

    private Address() { }

    public Address(string street, string city, string postalCode, string country, string houseNumber)
    {
        ValidateAddress(street, city, postalCode, country, houseNumber);

        Street = street;
        City = city;
        PostalCode = postalCode;
        Country = country;
        HouseNumber = houseNumber;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Address other) return false;
        return Street == other.Street &&
               City == other.City &&
               PostalCode == other.PostalCode &&
               Country == other.Country &&
               HouseNumber == other.HouseNumber;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, PostalCode, Country, HouseNumber);
    }

    public override string ToString()
    {
        return $"{Street} {HouseNumber}, {PostalCode} {City}, {Country}";
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
