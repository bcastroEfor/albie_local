﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authorization;

namespace ActioBP.Identity.PermissionN.RolesToPermission
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        //JLA
        //public HasPermissionAttribute(Permissions permission) : base(permission.ToString())
        public HasPermissionAttribute()
        { }
    }
}