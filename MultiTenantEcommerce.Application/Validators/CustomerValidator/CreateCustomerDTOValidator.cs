using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Customer;
using MultiTenantEcommerce.Application.Validators.AddressValidator;
using MultiTenantEcommerce.Application.Validators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Validators.CustomerValidator;
public class CreateCustomerDTOValidator : AbstractValidator<CreateCustomerDTO>
{
    public CreateCustomerDTOValidator()
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        RuleFor(x => x.Password).PasswordRules();

        RuleFor(x => x.PhoneNumber).PhoneNumberRules();

        RuleFor(x => x.CountryCode).CountryCodeRules();

        RuleFor(x => x.Address).SetValidator(new AddressDTOValidator());
    }
}
