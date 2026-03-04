using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Platform.Identity.Commands.Auth.LoginUser;
public record AuthUserResponseDTO(string AccessToken,
    //string RefreshToken,
    Guid Id,
    string Name,
    string Email);
