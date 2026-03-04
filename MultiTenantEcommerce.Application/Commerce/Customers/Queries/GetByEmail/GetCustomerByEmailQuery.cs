using MediatR;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetByEmail;

public record GetCustomerByEmailQuery(
    string Email) : IRequest<CustomerResponseDTO>;