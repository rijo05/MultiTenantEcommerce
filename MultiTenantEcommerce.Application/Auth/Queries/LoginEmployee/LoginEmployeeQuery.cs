using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Auth.Queries.LoginEmployee;
public record LoginEmployeeQuery(
    string Email,
    string Password) : IQuery<AuthEmployeeResponseDTO>;
