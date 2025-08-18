using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.OrderItemValidator;

public class CreateOrderItemDTOValidator : AbstractValidator<CreateOrderItemDTO>
{
    public CreateOrderItemDTOValidator()
    {
        RuleFor(x => x.Id).GuidRules();

        RuleFor(x => x.Quantity).QuantityRules();
    }
}
