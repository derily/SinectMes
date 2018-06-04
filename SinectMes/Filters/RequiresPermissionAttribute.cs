using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SinectMes.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinectMes.Filters
{
    public class RequiresPermissionAttribute:TypeFilterAttribute
    {
        public RequiresPermissionAttribute(params Permission[] permissions) : base(typeof(RequiresPermissionAttributeImpl))
        {
            Arguments = new[] { new PermissionRequirement(permissions) };
        }

        private class RequiresPermissionAttributeImpl : Attribute, IAsyncResourceFilter
        {
            private readonly ILogger _logger;
            private readonly IAuthorizationService _authService;
            private readonly PermissionRequirement _permissionRequirement;

            public RequiresPermissionAttributeImpl(ILogger<RequiresPermissionAttribute> logger,
                                                   IAuthorizationService authService,
                                                   PermissionRequirement permissionRequirement)
            {
                _logger = logger;
                _authService = authService;
                _permissionRequirement = permissionRequirement;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                _logger.LogTrace("Executing RequiresPermissionAttributeImpl filter");

                var result = await _authService.AuthorizeAsync(context.HttpContext.User, context.ActionDescriptor.ToString(), _permissionRequirement);
                if (!result.Succeeded)
                {
                    context.Result = new ChallengeResult();
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
