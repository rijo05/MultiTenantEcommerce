using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Domain.Users.Entities;

namespace MultiTenantEcommerce.Application.Users.Employees.Mappers;

public class EmployeeMapper
{
    private readonly HateoasLinkService _hateoasLinkService;

    public EmployeeMapper(HateoasLinkService hateoasLinkService)
    {
        _hateoasLinkService = hateoasLinkService;
    }

    public EmployeeResponseDTO ToEmployeeResponseDTO(Employee Employee)
    {
        return new EmployeeResponseDTO
        {
            Id = Employee.Id,
            Name = Employee.Name,
            Email = Employee.Email.Value,
            Role = Employee.Role.ToString(),
            IsActive = Employee.IsActive,
            CreatedAt = Employee.CreatedAt,
            UpdatedAt = Employee.UpdatedAt,
            //Links = GenerateLinks(Employee)
        };
    }

    public List<EmployeeResponseDTO> ToEmployeeResponseDTOList(List<Employee> Employees)
    {
        return Employees.Select(x => ToEmployeeResponseDTO(x)).ToList();
    }


    //private Dictionary<string, object> GenerateLinks(Employee Employee)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                Employee.Id,
    //                "Employees",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}
}
