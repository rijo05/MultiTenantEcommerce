using FluentValidation;
using MultiTenantEcommerce.Application.Catalog.DTOs.Product;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Common.Validators;

namespace MultiTenantEcommerce.Application.Catalog.Products.Validators;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator() 
    {
        RuleFor(x => x.Name).NameRules();

        RuleFor(x => x.Description).DescriptionRules();

        RuleFor(x => x.Price).PriceRules();

        RuleFor(x => x.CategoryId).GuidRules();

        RuleFor(x => x.Quantity).StockRules()
            .When(x => x.Quantity.HasValue);

        RuleFor(x => x.MinimumQuantity).MinimumStockLevelRules()
            .When(x => x.MinimumQuantity.HasValue);
    }
}
