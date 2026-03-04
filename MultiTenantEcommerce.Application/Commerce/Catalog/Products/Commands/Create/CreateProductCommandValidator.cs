using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NameRules();

        RuleFor(x => x.Description).DescriptionRules();

        RuleFor(x => x.Price).PriceRules();

        RuleFor(x => x.CategoryId).GuidRules();
    }
}