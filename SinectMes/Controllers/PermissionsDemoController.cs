using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SinectMes.Filters;
using SinectMes.Security;

namespace SinectMes.Controllers
{
    public class PermissionsDemoController : Controller
    {
        [RequiresPermission(Permission.Permission1)]
        public IActionResult TestPermission1()
        {
            return Content("Success!");
        }

        [RequiresPermission(Permission.Permission2)]
        public IActionResult TestPermission2()
        {
            return Content("Success!");
        }

        [RequiresPermission(Permission.Permission1, Permission.Permission2)]
        public IActionResult TestPermission12()
        {
            return Content("Success!");
        }
    }
}