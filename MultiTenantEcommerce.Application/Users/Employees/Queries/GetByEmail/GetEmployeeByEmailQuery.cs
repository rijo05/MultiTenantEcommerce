using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;


namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
public record GetEmployeeByEmailQuery(
    string Email) : IQuery<EmployeeResponseDTO>;
