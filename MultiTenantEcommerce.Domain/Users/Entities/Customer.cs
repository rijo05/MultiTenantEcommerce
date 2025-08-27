using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Customer : UserBase
{
    public Address Address { get; private set; }
    public Password Password { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private Customer() { }
    public Customer(Guid tenantId, string name, Email email, Password password, Address address, PhoneNumber phoneNumber) : base(tenantId, name, email)
    {
        Address = address;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public void UpdatePassword(string newPassword)
    {
        Password.UpdatePassword(newPassword);
        SetUpdatedAt();
    }

    public void UpdateAddress(string address)
    {
        //TODO()
        SetUpdatedAt();
    }
}
