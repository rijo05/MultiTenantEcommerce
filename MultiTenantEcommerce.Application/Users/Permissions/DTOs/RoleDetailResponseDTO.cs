using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Permissions.DTOs;
public class RoleDetailResponseDTO : RoleResponseDTO
{
    public List<PermissionResponseDTO> Permissions { get; init; }
}
