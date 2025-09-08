using FluentValidation;
using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Common.Helpers.Validators.AddressValidator;

public class AddressDTOValidator : AbstractValidator<CreateAddressDTO>
{
    public AddressDTOValidator()
    {
        RuleFor(x => x.Street).StreetRules();

        RuleFor(x => x.City).CityRules();

        RuleFor(x => x.PostalCode).PostalCodeRules();

        RuleFor(x => x.Country).CountryRules();

        RuleFor(x => x.HouseNumber).HouseNumberRules();
    }
}
