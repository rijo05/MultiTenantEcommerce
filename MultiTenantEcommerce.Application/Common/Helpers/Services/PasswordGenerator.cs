using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiTenantEcommerce.Application.Common.Helpers.Services;
public class PasswordGenerator
{
    public string GenerateRandomPassword(int length = 12)
    {
        if (length < 8)
            throw new ArgumentException("Password length must be at least 8 characters.");

        const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string Digits = "0123456789";
        const string Special = "#?!@$%^&*-";
        const string AllCharacters = UpperCase + LowerCase + Digits + Special;

        var builder = new StringBuilder();

        builder.Append(GetRandomChar(UpperCase));
        builder.Append(GetRandomChar(LowerCase));
        builder.Append(GetRandomChar(Digits));
        builder.Append(GetRandomChar(Special));

        for (int i = builder.Length; i < length; i++)
        {
            builder.Append(GetRandomChar(AllCharacters));
        }

        var chars = builder.ToString().ToCharArray();
        Shuffle(chars);

        var password = new string(chars);

        if (!Regex.IsMatch(password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$"))
            GenerateRandomPassword();

        return password;
    }

    private char GetRandomChar(string source)
    {
        var bytes = new byte[1];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return source[bytes[0] % source.Length];
    }

    private void Shuffle(char[] array)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                var bytes = new byte[1];
                rng.GetBytes(bytes);
                int j = bytes[0] % (i + 1);

                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
