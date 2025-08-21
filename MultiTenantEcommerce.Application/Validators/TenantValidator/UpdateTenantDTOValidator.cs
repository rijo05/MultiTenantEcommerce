using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Tenant;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.TenantValidator;
public class UpdateTenantDTOValidator : AbstractValidator<UpdateTenantDTO>
{
    public UpdateTenantDTOValidator()
    {
        RuleFor(x => x.CompanyName).NameRules()
            .When(x => !string.IsNullOrEmpty(x.CompanyName));
    }
}
