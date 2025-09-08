using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Customer : UserBase
{
    public Address Address { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private Customer() { }
    public Customer(Guid tenantId, string name, Email email, Password password, Address address, PhoneNumber phoneNumber)
        : base(tenantId, name, password, email)
    {
        Address = address;
        PhoneNumber = phoneNumber;
    }
    public void UpdateCustomer(
        string? name,
        string? email,
        string? password
        /*string? phoneNumber,
        //string? countryCode,
        //string? address = null*/)
    {
        if (!string.IsNullOrWhiteSpace(name))
            UpdateName(name);

        if (email != null)
            UpdateEmail(email);

        if (password != null)
            UpdatePassword(password);

        //if (phoneNumber != null)
        //    UpdatePhoneNumber(phoneNumber);

        //if (address != null)
        //    UpdateAddress(address);

        //TODO() ver como atualizar address e phone number ##########
    }

    //private void UpdatePhoneNumber(PhoneNumber phoneNumber)
    //{
    //    if (phoneNumber == null) throw new Exception("Phone number cannot be null.");
    //    PhoneNumber = phoneNumber;
    //}

    //private void UpdateAddress(Address address)
    //{
    //    if (address == null) throw new Exception("Address cannot be null.");
    //    Address = address;
    //}
}
