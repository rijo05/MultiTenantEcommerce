using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Password
{
    public string Value { get; private set; }
    const string Pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

    private Password() { }
    public Password(string password)
    {
        ValidatePassword(password);
        Value = HashPassword(password);
    }

    public void UpdatePassword(string password)
    {
        ValidatePassword(password);
        VerifySamePassword(password);
        Value = HashPassword(password);
    }

    private void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password)) { throw new ArgumentNullException("Password nao pode ser null"); }

        if (!Regex.IsMatch(password, Pattern))
            throw new ArgumentException("Password cannot be null or empty.");
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private void VerifySamePassword(string newPassword)
    {
        if (BCrypt.Net.BCrypt.Verify(newPassword, Value))
            throw new Exception("The new password cannot be the same as the current one.");
    }
}
