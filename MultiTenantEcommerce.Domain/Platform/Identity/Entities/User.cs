using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Utilities.Guards;

namespace MultiTenantEcommerce.Domain.Platform.Identity.Entities;

public class User : BaseEntity
{
    private User()
    {
    }

    public User(string firstName, string lastName, Email email, string password, bool isPlatformAdmin)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        IsPlatformAdmin = isPlatformAdmin;
        EmailVerified = false;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public bool IsPlatformAdmin { get; private set; }
    public bool EmailVerified { get; private set; }

    public void UpdatePassword(string? password)
    {
        Password = password;
        SetUpdatedAt();
    }

    public void UpdateEmail(string? newEmail)
    {
        Email.UpdateEmail(newEmail);
        SetUpdatedAt();
    }
}