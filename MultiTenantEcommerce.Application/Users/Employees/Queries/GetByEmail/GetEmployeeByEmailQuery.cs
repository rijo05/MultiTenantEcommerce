using MultiTenantEcommerce.Application.Catalog.DTOs.Product;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;


namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
public record GetEmployeeByEmailQuery(
    string Email) : IQuery<EmployeeResponseDTO>;
