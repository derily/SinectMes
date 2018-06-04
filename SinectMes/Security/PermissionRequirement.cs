using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinectMes.Security
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public IEnumerable<Permission> RequiredPermissions { get; }

        public PermissionRequirement(IEnumerable<Permission> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }
}
