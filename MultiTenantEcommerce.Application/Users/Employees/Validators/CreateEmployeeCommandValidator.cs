using FluentValidation;
using MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;

namespace MultiTenantEcommerce.Application.Users.Employees.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        //RuleFor(x => x.Role).RoleRules();
    }
}
