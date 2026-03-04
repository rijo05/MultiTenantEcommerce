using FluentValidation;
using MultiTenantEcommerce.Application.Common.Helpers.Validators;
using MultiTenantEcommerce.Shared.Application.Validators;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.CreateMember;

public class CreateTenantMemberCommandValidator : AbstractValidator<CreateTenantMemberCommand>
{
    public CreateTenantMemberCommandValidator()
    {
        RuleFor(x => x.Name).UserNameRules();

        RuleFor(x => x.Email).EmailRules();

        //RuleFor(x => x.Role).RoleRules();
    }
}