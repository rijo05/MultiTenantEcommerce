using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NameRules()
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description).DescriptionRules()
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Price).PriceRules()
            .When(x => x.Price.HasValue);

        RuleFor(x => x.CategoryId).GuidRules()
            .When(x => x.CategoryId.HasValue);
    }
}