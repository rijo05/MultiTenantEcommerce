using FluentValidation;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.DTOs.Category;
using MultiTenantEcommerce.Application.Common.Validators;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NameRules();

        RuleFor(x => x.Description).DescriptionRules();
    }
}
