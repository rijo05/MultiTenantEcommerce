using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.LoginCustomer;

public record LoginCustomerQuery(
    string Email,
    string Password) : IQuery<AuthCustomerResponseDTO>;