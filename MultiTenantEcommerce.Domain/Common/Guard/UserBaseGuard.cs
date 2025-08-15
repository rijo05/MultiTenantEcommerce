namespace MultiTenantEcommerce.Domain.Common.Guard;

public static class UserBaseGuard
{
    public static void AgainstNullOrEmptyName(string name)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 50, nameof(name));
        GuardCommon.AgainstInvalidFormat(name, @"^[a-zA-Z\s]+$", nameof(name));
    }

    public static void AgainstInvalidEmail(string email)
    {
        GuardCommon.AgainstNullOrEmpty(email, nameof(email));

        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        GuardCommon.AgainstInvalidFormat(email, emailPattern, nameof(email));
    }
}
