using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Email
{
    public string Value { get; private set; }
    private const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private Email() { }
    public Email(string email)
    {
        ValidateEmail(email);
        Value = email;
    }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);
        VerifySameEmail(email);
        Value = email; 
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException("Email cannot be null or empty."); }

        if (!Regex.IsMatch(email, Pattern))
            throw new ArgumentException("Email has invalid format.");
    }

    private void VerifySameEmail(string email)
    {
        if (email.Equals(Value, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("New email can't be equal to previous email.");
    }
}
