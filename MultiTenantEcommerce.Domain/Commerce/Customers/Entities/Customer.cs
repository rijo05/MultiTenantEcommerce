using MultiTenantEcommerce.Domain.Commerce.Customers.Events;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Customers.Entities;

public class Customer : TenantBase
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool EmailVerified { get; private set; }
    public IReadOnlyCollection<CustomerAddress> Addresses => _addresses.AsReadOnly();
    private readonly List<CustomerAddress> _addresses = new();

    public Guid? DefaultAddressId { get; private set; }

    private Customer() { }

    public Customer(Guid tenantId, string firstName, string lastName, Email email, string passwordHash)
        : base(tenantId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        EmailVerified = false;

        AddDomainEvent(new CustomerRegisteredEvent(TenantId, Id, email.Value, firstName));
    }

    public void AddAddress(string street, string city, string postalCode, string country, string houseNumber)
    {
        if (_addresses.Count > 5)
            throw new Exception("Limite maximo de enderecos atingida");

        bool alreadyExists = _addresses.Any(a =>
            a.Street == street &&
            a.HouseNumber == houseNumber &&
            a.PostalCode == postalCode);

        if (alreadyExists)
            throw new Exception("Esta morada já se encontra registada.");

        var newAddress = new CustomerAddress(street, city, postalCode, country, houseNumber);

        if (_addresses.Count == 0)
            DefaultAddressId = newAddress.Id;

        _addresses.Add(newAddress);
        SetUpdatedAt();
    }

    public void RemoveAddress(Guid addressId)
    {
        var address = _addresses.FirstOrDefault(x => x.Id == addressId);
        if (address != null)
        {
            _addresses.Remove(address);

            if (DefaultAddressId == addressId) DefaultAddressId = null;

            SetUpdatedAt();
        }
    }

    public void SetDefaultAddress(Guid addressId)
    {
        if (!_addresses.Any(a => a.Id == addressId))
            throw new Exception("Address not found");

        DefaultAddressId = addressId;
        SetUpdatedAt();
    }

    //public void UpdateCustomer(
    //string? name,
    //string? email,
    //string? password)
    //{
    //    if (!string.IsNullOrWhiteSpace(name))
    //        UpdateName(name);

    //    if (email != null)
    //        UpdateEmail(email);

    //    if (password != null)
    //        UpdatePassword(password);

    //    //if (phoneNumber != null)
    //    //    UpdatePhoneNumber(phoneNumber);

    //    //if (address != null)
    //    //    UpdateAddress(address);

    //    //TODO() ver como atualizar address e phone number ##########
    //}

    //public void UpdatePassword(string? password)
    //{
    //    Password.UpdatePassword(password);
    //    SetUpdatedAt();
    //}

    //public void UpdateEmail(string? newEmail)
    //{
    //    Email.UpdateEmail(newEmail);
    //    SetUpdatedAt();
    //}
}