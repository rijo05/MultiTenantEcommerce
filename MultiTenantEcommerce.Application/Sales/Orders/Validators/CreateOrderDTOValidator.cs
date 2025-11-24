namespace MultiTenantEcommerce.Application.Sales.Orders.Validators;

//public class CreateOrderDTOValidator : AbstractValidator<CreateOrderDTO>
//{
//    public CreateOrderDTOValidator()
//    {
//        RuleFor(x => x.CustomerId).GuidRules();

//        RuleFor(x => x.Address).SetValidator(new AddressDTOValidator());

//        RuleFor(x => x.Items)
//            .NotEmpty().WithMessage("Order must have at least one item.")
//            .ForEach(item =>
//            {
//                item.SetValidator(new CreateOrderItemDTOValidator());
//            });
//    }
//}
