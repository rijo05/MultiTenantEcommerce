using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Update;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Name).UserNameRules()
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Email).EmailRules()
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password).PasswordRules()
            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.PhoneNumber).PhoneNumberRules()
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.CountryCode).CountryCodeRules()
            .When(x => !string.IsNullOrEmpty(x.CountryCode));

        //TODO() UPDATE ADDRESS ##############################
    }
}