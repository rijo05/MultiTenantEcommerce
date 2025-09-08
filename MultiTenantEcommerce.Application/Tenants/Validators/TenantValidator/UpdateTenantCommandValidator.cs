using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;

namespace MultiTenantEcommerce.Application.Tenants.Validators.TenantValidator;
public class UpdateTenantCommandValidator : AbstractValidator<UpdateTenantCommand>
{
    public UpdateTenantCommandValidator()
    {
        RuleFor(x => x.CompanyName).NameRules()
            .When(x => !string.IsNullOrEmpty(x.CompanyName));
    }
}
