using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.ProductValidator;

public class UpdateProductDTOValidator : AbstractValidator<UpdateProductDTO>
{
    public UpdateProductDTOValidator() 
    {
        RuleFor(x => x.Name).NameRules()
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description).DescriptionRules()
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Price).PriceRules()
            .When(x => x.Price.HasValue);

        RuleFor(x => x.CategoryId.Value).GuidRules()
            .When(x => x.CategoryId.HasValue);
    }
}
