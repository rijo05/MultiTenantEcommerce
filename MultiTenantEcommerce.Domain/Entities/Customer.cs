using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Customer : UserBase
{
    public Guid Id { get; private set; }
    public Address Address { get; private set; }
    public Password Password { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }


    public Customer(Guid tenantId, string name, Email email, Password password, Address address, PhoneNumber phoneNumber) : base(tenantId, name, email)
    {
        Id = Guid.NewGuid();
        Address = address;
        Password = password;
        PhoneNumber = phoneNumber;
    }

    public void UpdatePassword(string newPassword)
    {
        Password.UpdatePassword(newPassword);
        UpdatedAt = DateTime.UtcNow;
    }
}
