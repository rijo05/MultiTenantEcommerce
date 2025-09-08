using FluentValidation;
using MultiTenantEcommerce.Application.Auth.Commands.CreateCustomer;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Application.Common.Helpers.Validators.AddressValidator;

namespace MultiTenantEcommerce.Application.Users.Customers.Validators;
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
