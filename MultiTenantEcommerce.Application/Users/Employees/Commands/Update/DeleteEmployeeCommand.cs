using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
public record DeleteEmployeeCommand(
    Guid id) : ICommand<Unit>;
