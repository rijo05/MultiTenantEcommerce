using MultiTenantEcommerce.Application.DTOs.Employees;

namespace MultiTenantEcommerce.Application.Interfaces;

public interface IEmployeeService
{
    public Task<List<EmployeeResponseDTO>> GetFilteredEmployeesAsync(EmployeeFilterDTO EmployeeFilter);
    public Task<EmployeeResponseDTO?> GetEmployeeByIdAsync(Guid id);
    public Task<EmployeeResponseDTO?> GetEmployeeByEmailAsync(string email);


    public Task<EmployeeResponseDTO> AddEmployeeAsync(CreateEmployeeDTO EmployeeDTO);
    public Task DeleteEmployeeAsync(Guid id);
    public Task<EmployeeResponseDTO> UpdateEmployeeAsync(Guid id, UpdateEmployeeDTO updatedEmployeeDTO);
}
