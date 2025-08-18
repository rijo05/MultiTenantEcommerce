using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.ProductValidator;

public class CreateProductDTOValidator : AbstractValidator<CreateProductDTO>
{
    public CreateProductDTOValidator() 
    {
        RuleFor(x => x.ProductDTO.Name).NameRules();

        RuleFor(x => x.ProductDTO.Description).DescriptionRules();

        RuleFor(x => x.ProductDTO.Price).PriceRules();

        RuleFor(x => x.ProductDTO.CategoryId).GuidRules();

        RuleFor(x => x.stockDTO.Quantity).StockRules();

        RuleFor(x => x.stockDTO.MinimumQuantity).MinimumStockLevelRules();
    }
}
