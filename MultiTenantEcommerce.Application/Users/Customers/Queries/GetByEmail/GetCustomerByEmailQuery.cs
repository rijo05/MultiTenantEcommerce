﻿using MediatR;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetByEmail;
public record GetCustomerByEmailQuery(
    string Email) : IRequest<CustomerResponseDTO>;
