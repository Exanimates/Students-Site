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
        readonly ITeacherService _teacherService;
        readonly IStudentService _studentService;

        public AccountController(IUserService userService, ITeacherService teacherService, IStudentService studentService)
        {
            _userService = userService;
            _teacherService = teacherService;
            _studentService = studentService;
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
                var user = new UserModel
                {
                    Login = userBll.Login,
                    RoleId = userBll.RoleId,
                };

                await Authenticate(user);

                return RedirectToAction("Index", "Home");
            }

            return StatusCode(500, "Такого пользователя не существует");
        }

        private async Task Authenticate(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleId.ToString())
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