using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Password
{
    private string Value;
    private const string Pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";

    private Password() { }
    public Password(string password)
    {
        ValidatePassword(password);
        Value = HashPassword(password);
    }

    public void UpdatePassword(string password)
    {
        ValidatePassword(password);

        if (VerifySamePassword(password))
            throw new Exception("Password cant be equal to previous one");

        Value = HashPassword(password);
    }

    public bool VerifySamePassword(string newPassword)
    {
        if (BCrypt.Net.BCrypt.Verify(newPassword, Value))
            return true;

        return false;
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
}
