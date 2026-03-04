using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

public interface IAddressValidator
{
    public Task<AddressValidationResult> IsAddressValid(Address address);
}