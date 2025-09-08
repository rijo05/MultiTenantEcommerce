using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Auth.Queries.LoginCustomer;
public record LoginCustomerQuery(
    string Email,
    string Password) : IQuery<AuthCustomerResponseDTO>;
