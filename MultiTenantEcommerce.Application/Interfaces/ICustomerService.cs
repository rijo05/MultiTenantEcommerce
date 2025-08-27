using MultiTenantEcommerce.Application.DTOs.Customer;
using MultiTenantEcommerce.Application.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Interfaces;
public interface ICustomerService
{
    public Task<CustomerResponseDTO?> GetCustomerByIdAsync(Guid id);
    public Task<CustomerResponseDTO?> GetCustomerByEmailAsync(string email);
    public Task<CustomerResponseDTO?> GetCustomerByPhoneNumberAsync(string number);

    public Task<CustomerResponseDTO> AddCustomerAsync(CreateCustomerDTO customerDTO);
    public Task DeleteCustomerAsync(Guid id);
    public Task<CustomerResponseDTO> UpdateCustomerAsync(Guid id, UpdateCustomerDTO updateCustomer);
}
