using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.UpdateSettings;

public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.CompanyName).NameRules()
            .When(x => !string.IsNullOrEmpty(x.CompanyName));
    }
}