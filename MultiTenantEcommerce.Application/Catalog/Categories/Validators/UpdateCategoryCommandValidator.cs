using FluentValidation;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Validators;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NameRules()
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description).DescriptionRules()
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
