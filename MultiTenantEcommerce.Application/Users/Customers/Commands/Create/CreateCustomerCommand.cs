using MediatR;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Create;
public record CreateCustomerCommand(
    string Name,
    string Email,
    string Password,
    string CountryCode,
    string PhoneNumber,
    CreateAddressDTO Address) : ICommand<CustomerResponseDTO>;
