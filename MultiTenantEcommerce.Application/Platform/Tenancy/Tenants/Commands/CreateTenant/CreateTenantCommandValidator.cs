using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.CompanyName).UserNameRules();
    }
}