using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NameRules();

        RuleFor(x => x.Description).DescriptionRules();
    }
}