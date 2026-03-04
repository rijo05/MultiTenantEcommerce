using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Platform.Identity.Users.Commands.Update;

public class UpdateTenantMemberCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateTenantMemberCommandValidator()
    {
        RuleFor(x => x.Name).UserNameRules()
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Email).EmailRules()
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Password).PasswordRules()
            .When(x => !string.IsNullOrEmpty(x.Password));

        //RuleFor(x => x.Role).RoleRules()
        //                    .When(x => !string.IsNullOrEmpty(x.Role));
    }
}