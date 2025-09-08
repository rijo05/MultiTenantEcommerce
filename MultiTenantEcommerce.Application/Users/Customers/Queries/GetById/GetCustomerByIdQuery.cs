using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;
public record GetCustomerByIdQuery(
    Guid Id) : IRequest<CustomerResponseDTO>;
