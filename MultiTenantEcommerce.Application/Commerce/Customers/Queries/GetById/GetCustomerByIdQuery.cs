using MediatR;
using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetById;

public record GetCustomerByIdQuery(
    Guid Id) : IRequest<CustomerResponseDTO>;