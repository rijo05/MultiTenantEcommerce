using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.Validators.Common;

namespace MultiTenantEcommerce.Application.Validators.EmployeeValidator;

public class CreateEmployeeDTOValidator : AbstractValidator<CreateEmployeeDTO>
{
    public CreateEmployeeDTOValidator() 
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        RuleFor(x => x.Password).PasswordRules();

        RuleFor(x => x.Role).RoleRules();
    }
}
