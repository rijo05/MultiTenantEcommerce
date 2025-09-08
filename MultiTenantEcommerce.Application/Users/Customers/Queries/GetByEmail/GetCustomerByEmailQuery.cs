using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetByEmail;
public record GetCustomerByEmailQuery(
    string Email) : IRequest<CustomerResponseDTO>;
