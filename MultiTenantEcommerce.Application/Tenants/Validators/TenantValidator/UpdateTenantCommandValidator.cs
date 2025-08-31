using FluentValidation;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Tenancy.DTOs.Tenant;
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
