using FluentValidation;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Tenancy.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Create;

namespace MultiTenantEcommerce.Application.Tenants.Validators.TenantValidator;
public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.CompanyName).UserNameRules();
    }
}
