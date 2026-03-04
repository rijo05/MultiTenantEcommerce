using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Platform.Identity.Commands.Auth.CreateUser;
public record CreateUserCommand(
    string FirstName, 
    string LastName,
    string Email,
    string Password) : ICommand<Unit>;
