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
public class UpdateCustomerDTOValidator : AbstractValidator<UpdateCustomerDTO>
{
    public UpdateCustomerDTOValidator()
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
