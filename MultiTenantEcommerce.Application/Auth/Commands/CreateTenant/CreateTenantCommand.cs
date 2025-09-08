using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
public record CreateTenantCommand(
    string CompanyName,
    string OwnerName,
    string OwnerEmail,
    string Password) : ICommand<AuthTenantResponse>;
