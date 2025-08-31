using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;
public record GetCustomerByIdQuery(
    Guid Id) : IRequest<CustomerResponseDTO>;
