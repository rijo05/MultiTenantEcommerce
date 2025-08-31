using MultiTenantEcommerce.Application.Catalog.DTOs.Product;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;
public record GetFilteredEmployeesQuery(
    string? Name,
    string? Role,
    string? Email,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<EmployeeResponseDTO>>;
