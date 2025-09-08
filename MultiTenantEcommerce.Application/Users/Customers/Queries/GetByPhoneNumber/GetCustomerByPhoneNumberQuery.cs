using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetByPhoneNumber;
public record GetCustomerByPhoneNumberQuery(
    string CountryCode,
    string Number) : IRequest<CustomerResponseDTO>;

