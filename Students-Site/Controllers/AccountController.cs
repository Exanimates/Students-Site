using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Services;
using Students_Site.Models.Login;
using Students_Site.Models.Users;

namespace Students_Site.WEB.Controllers
{
    public class AccountController : Controller
    {
        readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            UserBLL userBll = _userService.GetUsers().FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                
            if (userBll != null)
            {
                var user = new UserBLL
                {
                    Login = userBll.Login,
                    RoleId = userBll.RoleId
                };

                await Authenticate(model.Login);

                return RedirectToAction("Index", "Home");
            }

            return Content("Такого пользователя не существует");
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}