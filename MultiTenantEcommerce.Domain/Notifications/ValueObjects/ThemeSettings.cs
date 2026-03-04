using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MultiTenantEcommerce.Domain.Notifications.ValueObjects;

public record HexColor
{
    private HexColor(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<HexColor> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, "^#(?:[0-9a-fA-F]{3}){1,2}$"))
            return Result.Failure<HexColor>(new Error("Color.Invalid", "O formato da cor Hexadecimal é inválido."));

        return Result.Success(new HexColor(value));
    }
}

public record ThemeSettings(HexColor PrimaryColor, HexColor SecondaryColor, string? LogoUrl)
{
    public static ThemeSettings Default()
    {
        return new ThemeSettings(HexColor.Create("#000000").Value, HexColor.Create("#FFFFFF").Value, null);
    }
}