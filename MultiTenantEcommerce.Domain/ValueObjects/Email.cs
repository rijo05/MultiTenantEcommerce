using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Email
{
    public string Value { get; private set; }
    const string Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

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
        if(email == Value)
            throw new Exception("New email can't be equal to previous email.");
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        var other = (Email)obj;
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
