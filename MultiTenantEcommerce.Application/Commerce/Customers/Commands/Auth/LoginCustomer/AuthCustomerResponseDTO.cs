namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.LoginCustomer;

public record AuthCustomerResponseDTO(
    string AccessToken,
    //string RefreshToken,
    Guid Id,
    string Name,
    string Email);