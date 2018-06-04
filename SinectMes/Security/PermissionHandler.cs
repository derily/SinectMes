using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SinectMes.Identity;
using SinectMes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinectMes.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        #region original implement
        //private readonly IUserPermissionsRepository permissionsRepository;
        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        //{
        //    if (context.User == null)
        //    {
        //        return Task.CompletedTask;
        //    }
        //    bool hasPermission= permissionsRepository.CheckPermissionForUser(context.User, requirement.Permission);
        //    if (hasPermission)
        //    {
        //        context.Succeed(requirement);
        //    }

        //    return Task.CompletedTask;
        //}
        #endregion

        private readonly ILogger _logger;
        private readonly DemoUserManager<ApplicationUser> _userManager;

        public PermissionHandler(ILogger<PermissionHandler> logger, DemoUserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = await _userManager.GetUserAsync(context.User);
            var currentUserPermissions = await _userManager.GetUserPermissionsAsync(user);

            var authorized = requirement.RequiredPermissions.AsParallel().All(rp => currentUserPermissions.Contains(rp)); // TODO: load permissions into context.User 
            if (authorized) context.Succeed(requirement);
        }
    }
}
