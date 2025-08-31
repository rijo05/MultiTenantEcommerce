using FluentValidation;
using MultiTenantEcommerce.Application.Catalog.DTOs.Product;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
using MultiTenantEcommerce.Application.Common.Validators;

namespace MultiTenantEcommerce.Application.Catalog.Products.Validators;

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
