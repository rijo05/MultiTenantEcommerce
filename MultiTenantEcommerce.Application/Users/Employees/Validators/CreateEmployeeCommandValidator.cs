using FluentValidation;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Create;

namespace MultiTenantEcommerce.Application.Users.Employees.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator() 
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        RuleFor(x => x.Password).PasswordRules();

        RuleFor(x => x.Role).RoleRules();
    }
}
