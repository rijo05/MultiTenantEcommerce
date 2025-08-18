using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.EmployeeValidator;

public class UpdateEmployeeDTOValidator : AbstractValidator<UpdateEmployeeDTO>
{
    public UpdateEmployeeDTOValidator()
    {
        RuleFor(x => x.Name).UserNameRules()
                            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Email).EmailRules()
                            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password).PasswordRules()
                            .When(x => !string.IsNullOrEmpty(x.Password));

        RuleFor(x => x.Role).RoleRules()
                            .When(x => !string.IsNullOrEmpty(x.Role));
    }
}
