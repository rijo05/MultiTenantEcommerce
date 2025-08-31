using FluentValidation;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Common.Validators.AddressValidator;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Create;
using MultiTenantEcommerce.Application.Users.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
