using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
public record DeleteEmployeeCommand(
    Guid id) : ICommand<Unit>;
