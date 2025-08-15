namespace MultiTenantEcommerce.Domain.Common.Guard;

public static class GuardCommon
{
    public static void AgainstNullOrEmpty(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{parameterName}' cannot be null, empty or whitespace.");
    }
    public static void AgainstNull(object value, string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName, $"'{parameterName}' cannot be null.");
    }

    public static void AgainstNegativeOrZero(decimal value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' must be greater than zero.");
    }
    public static void AgainstNegativeOrZero(int value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' must be greater than zero.");
    }
    public static void AgainstNegative(int value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' must be greater than zero.");
    }
    public static void AgainstOutOfRange(int value, int min, int max, string parameterName)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' must be between {min} and {max}.");
    }

    public static void AgainstMaxLength(string value, int maxLength, string parameterName)
    {
        if (value is not null && value.Length > maxLength)
            throw new ArgumentException($"'{parameterName}' cannot exceed {maxLength} characters.");
    }

    public static void AgainstTooShort(string value, string parameterName, int minLength)
    {
        if (value is null || value.Length < minLength)
            throw new ArgumentException($"{parameterName} tem de ter no mínimo {minLength} caracteres", parameterName);
    }

    public static void AgainstInvalidFormat(string value, string pattern, string parameterName)
    {
        if (value is not null && !System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
            throw new ArgumentException($"'{parameterName}' has invalid format.");
    }

    public static void AgainstLowDuration(TimeSpan duration, string parameterName)
    {
        if (duration < TimeSpan.FromHours(1))
            throw new ArgumentOutOfRangeException(parameterName, $"'{parameterName}' must be at least 1 hour.");
    }

    public static void AgainstEmptyGuid(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(parameterName, $"{parameterName} cannot be empty GUID.");
        }
    }
}
