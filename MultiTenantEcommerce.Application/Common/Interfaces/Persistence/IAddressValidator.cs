using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IAddressValidator
{
    public Task<AddressValidationResult> IsAddressValid(Address address);
}
