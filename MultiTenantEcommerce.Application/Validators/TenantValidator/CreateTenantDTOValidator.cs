using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Tenant;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.TenantValidator;
public class CreateTenantDTOValidator : AbstractValidator<CreateTenantDTO>
{
    public CreateTenantDTOValidator()
    {
        RuleFor(x => x.CompanyName).UserNameRules();
    }
}
