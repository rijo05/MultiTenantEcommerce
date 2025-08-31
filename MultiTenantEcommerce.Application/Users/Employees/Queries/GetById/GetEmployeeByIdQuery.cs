using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
public record GetEmployeeByIdQuery(
    Guid EmployeeId) : IQuery<EmployeeResponseDTO>;
