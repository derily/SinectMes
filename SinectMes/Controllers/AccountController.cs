using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SinectMes.Models;
using Microsoft.Extensions.Logging;
using SinectMes.ViewModels;
using System.Security.Claims;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SinectMes.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl=null){
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            if (_signInManager.IsSignedIn(User))
            {
               await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl=null){
            ViewData["ReturnUrl"] = returnUrl;

           // _roleManager.AddClaimAsync(new ApplicationRole(),new System.Security.Claims.Claim()
            if(ModelState.IsValid){
                var result = await _signInManager.PasswordSignInAsync(model.UserName,
                                                                    model.Password, model.RememberMe, lockoutOnFailure: false);
                if(result.Succeeded){
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "用户名或密码不正确！");
                    return View(model);
                }
            }
            // if we got this far,something failed,rediplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize]
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
            return PartialView("_Modal");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleView role)
        {
            // var role = new ApplicationRole() { Name = "Guest",ChineseName="来宾" };
            IdentityResult result;
         
            if (role.Id != 0)
            {
                var existedRole = await _roleManager.FindByIdAsync(role.Id.ToString());
                existedRole.Name = role.Name;
                existedRole.ChineseName = role.ChineseName;
               result= await _roleManager.UpdateAsync(existedRole);
            }
            else
            {
                result = await _roleManager.CreateAsync(new ApplicationRole { Name = role.Name, ChineseName = role.ChineseName });
            }
            
           // await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("permission", "page.create"));
            if(result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                return Content(string.Join('\n', result.Errors));
            }
          
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            //if (role != null)
            //{

            //}
            //return Content("alert('ab');");
            ViewBag.Id = roleId;
            return PartialView("_Modal",role);
            
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        

       
    }
}
