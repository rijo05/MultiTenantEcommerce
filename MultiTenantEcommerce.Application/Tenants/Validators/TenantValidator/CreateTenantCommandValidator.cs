using FluentValidation;
using MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;

namespace MultiTenantEcommerce.Application.Tenants.Validators.TenantValidator;
public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.CompanyName).UserNameRules();
    }
}
