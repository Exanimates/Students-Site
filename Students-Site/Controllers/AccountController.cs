using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Encryption;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.Models.Account;
using Students_Site.Models.Users;

namespace Students_Site.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;
        private readonly IRoleService _roleService;

        public AccountController(IUserService userService, ITeacherService teacherService, IStudentService studentService, IRoleService roleService)
        {
            _userService = userService;
            _teacherService = teacherService;
            _studentService = studentService;
            _roleService = roleService;
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

            var userBll = _userService.GetAll().FirstOrDefault(u => u.Login == model.Login && Hash.Validate(model.Password, u.Salt, u.Password));


            if (userBll == null) throw new ValidationException("Такого пользователя не существует","");
            var user = new UserModel
            {
                UserId = userBll.Id,
                Login = userBll.Login,
                RoleId = userBll.RoleId,
                FirstName = userBll.FirstName,
                LastName = userBll.LastName,
                RoleName = userBll.RoleName
            };

            await Authenticate(user);

            return Json(new {result = "Redirect", url = Url.Action("Index", "Home")});

        }

        private async Task Authenticate(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName)
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            _teacherService.Dispose();
            _studentService.Dispose();
            _roleService.Dispose();
            _userService.Dispose();
            base.Dispose(disposing);
        }
    }
}