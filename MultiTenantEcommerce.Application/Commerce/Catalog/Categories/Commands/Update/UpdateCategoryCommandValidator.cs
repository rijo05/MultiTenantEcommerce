using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Update;

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