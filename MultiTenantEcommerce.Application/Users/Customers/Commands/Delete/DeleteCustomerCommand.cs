using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.DTOs.Customer;
using System.Windows.Input;


namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Delete;
public record DeleteCustomerCommand(
    Guid Id) : ICommand<Unit>;
