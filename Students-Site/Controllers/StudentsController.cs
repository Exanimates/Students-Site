using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.DAL.Entities;
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
            var students = _studentService.GetStudents().Select(s => new StudentModel
            {
                Id = s.Id,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Login = s.User.Login,
                Password = s.User.Password,
                RoleId = s.User.RoleId,

                Teachers = s.Teachers.Select(t => new TeacherModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            }).ToArray();

            return View(students);
        }


        public ActionResult MakeStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakeStudent(StudentModel student)
        {
            try
            {
                var studentBll = new UserBLL
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Login = student.Login,
                    Password = student.Password
                };
                _studentService.MakeStudent(studentBll, null, 2);
                return Content("Студент успешно зарегестирован");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }

            return View(student);
        }

        public IActionResult ShowStudent(int id)
        {
            var studentBll = _studentService.GetStudent(id);

            var student = new StudentModel
            {
                Id = studentBll.Id,
                FirstName = studentBll.User.FirstName,
                LastName = studentBll.User.LastName,
                Login = studentBll.User.Login,
                Password = studentBll.User.Password,
                RoleId = studentBll.User.RoleId,

                Teachers = studentBll.Teachers.Select(t => new TeacherModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            };

            return View(student);
        }
    }
}