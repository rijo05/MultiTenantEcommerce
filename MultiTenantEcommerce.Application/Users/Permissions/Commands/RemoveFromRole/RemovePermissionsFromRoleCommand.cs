﻿using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.RemoveFromRole;
public record RemovePermissionsFromRoleCommand(
    Guid roleId,
    List<Guid> permissions) : ICommand<RoleResponseDTO>;
