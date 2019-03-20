using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.Models.Students;
using Students_Site.Models.Teachers;

namespace Students_Site.Controllers
{
    public class StudentsController : Controller
    {
        IStudentService _studentService;
        IUserService _userService;
        ITeacherService _teacherService;

        public StudentsController(IStudentService studentService, IUserService userService, ITeacherService teacherService)
        {
            _studentService = studentService;
            _userService = userService;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            var studentsModels = _studentService.GetStudents().Select(s => new StudentModel
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
            });

            var students = new StudentIndexModel
            {
                StudentModels = studentsModels
            };

            return View(students);
        }


        public ActionResult MakeStudent()
        {
            var student = new StudentMakeModel
            {
                TeachersList = _teacherService.GetTeachers().Select(s => new TeacherModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    SubjectName = s.SubjectName,
                }).ToList()
            };

            return View(student);
        }

        [HttpPost]
        public ActionResult MakeStudent(StudentMakeModel student)
        {
            try
            {
                var userBll = new UserBLL
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Login = student.Login,
                    Password = student.Password
                };

                var studentBll = new StudentBLL
                {
                    User = userBll,

                    Teachers = student.TeachersList.Where(t => t.IsSelected).Select(s => new TeacherBLL
                    {
                        Id = s.Id,
                        UserId = s.UserId
                    })
                };

                _studentService.MakeStudent(studentBll);

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
                    LastName = t.User.LastName,
                    SubjectName = t.SubjectName
                })
            };

            return View(student);
        }
    }
}