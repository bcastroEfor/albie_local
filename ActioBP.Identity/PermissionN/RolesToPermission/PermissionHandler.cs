﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ActioBP.Identity.PermissionN.RolesToPermission
{
    //thanks to https://www.jerriepelser.com/blog/creating-dynamic-authorization-policies-aspnet-core/

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var permissionsClaim =
                context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
            // If user does not have the scope claim, get out of here
            if (permissionsClaim == null)
                return Task.CompletedTask;
            //JLA
            //if (permissionsClaim.Value.ThisPermissionIsAllowed(requirement.PermissionName))
            //    context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}