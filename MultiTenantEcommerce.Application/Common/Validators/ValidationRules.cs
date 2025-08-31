using FluentValidation;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Common.Validators;

public static class ValidationRules
{
    public static async Task ValidateAsync<T>(T entity, IValidator<T> validator)
    {
        var validationResults = await validator.ValidateAsync(entity);

        if (!validationResults.IsValid)
            throw new ValidationException(validationResults.Errors);
    }

    //Username - No numbers allowed
    public static IRuleBuilderOptions<T, string?> UserNameRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(50).WithMessage("Name cannot exceed 50 characters")
                    .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces.");
    }

    //Product/category/... name - Numbers allowed
    public static IRuleBuilderOptions<T, string?> NameRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");
    }

    //Optional big text - max 255 characters
    public static IRuleBuilderOptions<T, string?> DescriptionRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");
    }

    //Email
    public static IRuleBuilderOptions<T, string?> EmailRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format");
    }

    //Password
    public static IRuleBuilderOptions<T, string?> PasswordRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .NotEmpty().WithMessage("Password is required")
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$")
                    .WithMessage("Password must have at least one uppercase letter, one number and one special character.");
    }

    //Roles - check if it exists
    public static IRuleBuilderOptions<T, string?> RoleRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
                    .NotEmpty().WithMessage("Role is required.")
                    .Must(role => Enum.TryParse<RoleType>(role, true, out _))
                    .WithMessage("Invalid role.");
    }

    //Price - Greater then 0
    public static IRuleBuilderOptions<T, decimal?> PriceRules<T>(this IRuleBuilder<T, decimal?> ruleBuilder)
    {
        return ruleBuilder
                    .NotNull()
                    .GreaterThan(0).WithMessage("Price must be greater than zero");
    }

    //Price Opcional
    public static IRuleBuilderOptions<T, decimal> PriceRules<T>(this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
                    .GreaterThan(0).WithMessage("Price must be greater than zero");
    }

    //Quantity
    public static IRuleBuilderOptions<T, int> QuantityRules<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero");
    }

    //GUID válido
    public static IRuleBuilderOptions<T, string?> GuidRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Id is required.")
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("Id must be a valid GUID.");
    }

    //Stock
    public static IRuleBuilderOptions<T, int?> StockRules<T>(this IRuleBuilder<T, int?> ruleBuilder)
    {
        return ruleBuilder
                    .GreaterThanOrEqualTo(0).WithMessage("Stock must be greater or equal to zero");
    }

    //Stock Minimo
    public static IRuleBuilderOptions<T, int?> MinimumStockLevelRules<T>(this IRuleBuilder<T, int?> ruleBuilder)
    {
        return ruleBuilder  
                    .GreaterThanOrEqualTo(0).WithMessage("Minimum stock level must be greater or equal to zero");
    }

    //Rua
    public static IRuleBuilderOptions<T, string?> StreetRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(100).WithMessage("Street cannot exceed 100 characters");
    }

    //Cidade
    public static IRuleBuilderOptions<T, string?> CityRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(50).WithMessage("City cannot exceed 50 characters");
    }

    //Codigo Postal
    public static IRuleBuilderOptions<T, string?> PostalCodeRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Postal code is required.")
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters");
    }

    //Pais
    public static IRuleBuilderOptions<T, string?> CountryRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country cannot exceed 50 characters");
    }

    //Porta Casa
    public static IRuleBuilderOptions<T, string?> HouseNumberRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("House number is required.")
            .MaximumLength(10).WithMessage("House number cannot exceed 10 characters");
    }

    //Not Empty
    public static IRuleBuilderOptions<T, Guid> GuidRules<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEqual(Guid.Empty).WithMessage("Id must be valid.");
    }
    //Not Empty
    public static IRuleBuilderOptions<T, Guid?> GuidRules<T>(this IRuleBuilder<T, Guid?> ruleBuilder)
    {
        return ruleBuilder
            .NotEqual(Guid.Empty).WithMessage("Id must be valid.");
    }

    //Country Code - Phone Number
    public static IRuleBuilderOptions<T, string?> CountryCodeRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("CountryCode is required.")
            .MaximumLength(5).WithMessage("CountryCode cannot exceed 5 characters");
    }

    //Phone Number
    public static IRuleBuilderOptions<T, string?> PhoneNumberRules<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .MaximumLength(20).WithMessage("PhoneNumber cannot exceed 20 characters");
    }
}
