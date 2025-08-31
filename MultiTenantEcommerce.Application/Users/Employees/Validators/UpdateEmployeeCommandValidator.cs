using FluentValidation;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Users.DTOs.Employees;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;

namespace MultiTenantEcommerce.Application.Users.Employees.Validators;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
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
