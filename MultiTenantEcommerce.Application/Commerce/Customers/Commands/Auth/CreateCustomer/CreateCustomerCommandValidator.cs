using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Application.Common.Helpers.Validators.AddressValidator;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        RuleFor(x => x.Password).PasswordRules();

        RuleFor(x => x.PhoneNumber).PhoneNumberRules();

        RuleFor(x => x.CountryCode).CountryCodeRules();

        RuleFor(x => x.Address).SetValidator(new AddressDTOValidator());
    }
}