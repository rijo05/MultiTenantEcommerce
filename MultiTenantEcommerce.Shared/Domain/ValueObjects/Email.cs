using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Shared.Domain.ValueObjects;

public class Email
{
    private const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    private Email()
    {
    }

    public Email(string email)
    {
        ValidateEmail(email);
        Value = email;
    }

    public string Value { get; private set; }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);
        VerifySameEmail(email);
        Value = email;
    }

    private void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email cannot be null or empty.");

        if (!Regex.IsMatch(email, Pattern))
            throw new ArgumentException("Email has invalid format.");
    }

    private void VerifySameEmail(string email)
    {
        if (email.Equals(Value, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("New email can't be equal to previous email.");
    }
}