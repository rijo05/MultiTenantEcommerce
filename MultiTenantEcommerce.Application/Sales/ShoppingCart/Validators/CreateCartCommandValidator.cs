using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Create;
namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Validators;
public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.CustomerId).GuidRules();
    }
}
