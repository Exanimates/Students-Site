using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models;

namespace Students_Site.Controllers
{
    public class StudentsController : Controller
    {
        IStudentService _studentService;
        IUserService _userService;

        public StudentsController(IStudentService studentService, IUserService userService)
        {
            _studentService = studentService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var studentsBll = _studentService.GetStudents();

            var users = _userService.GetUsers();

            var students = studentsBll
                .Join(
                    users,
                    s => s.UserId,
                    u => u.Id,
                    (s, u) => new StudentModel
                    {
                        Id = s.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Login = u.Login,
                        Password = u.Password,
                        RoleId = u.RoleId
                    });

            return View(students);
        }
    }
}