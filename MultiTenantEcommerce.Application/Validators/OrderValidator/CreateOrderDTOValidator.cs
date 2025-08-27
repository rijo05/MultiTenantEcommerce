using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.Validators.AddressValidator;
using MultiTenantEcommerce.Application.Validators.Common;
using MultiTenantEcommerce.Application.Validators.OrderItemValidator;

namespace MultiTenantEcommerce.Application.Validators.OrderValidator;

public class CreateOrderDTOValidator : AbstractValidator<CreateOrderDTO>
{
    public CreateOrderDTOValidator()
    {
        RuleFor(x => x.CustomerId).GuidRules();

        RuleFor(x => x.Address).SetValidator(new AddressDTOValidator());

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Order must have at least one item.")
            .ForEach(item =>
            {
                item.SetValidator(new CreateOrderItemDTOValidator());
            });
    }
}
