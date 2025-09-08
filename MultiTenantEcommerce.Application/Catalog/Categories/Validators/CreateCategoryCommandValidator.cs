using FluentValidation;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NameRules();

        RuleFor(x => x.Description).DescriptionRules();
    }
}
