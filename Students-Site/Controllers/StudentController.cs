using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.Models.Student;
using Students_Site.Models.Teacher;

namespace Students_Site.Controllers
{
    public class StudentController : Controller
    {
        readonly IStudentService _studentService;
        readonly ITeacherService _teacherService;

        public StudentController(IStudentService studentService, ITeacherService teacherService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
        }

        public IActionResult Index()
        {
            var studentsModels = _studentService.GetAll().Select(s => new StudentModel
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
                    LastName = t.User.LastName,
                    Grade = t.Grade
                }),
                AveradeScore = s.AverageScore,
            });

            var students = new IndexModel
            {
                StudentModels = studentsModels
            };

            return View(students);
        }

        [Authorize]
        [Authorize(Roles = "1,3")]
        public ActionResult Create()
        {
            var student = new StudentMakeModel
            {
                TeachersList = _teacherService.GetAll().Select(t => new TeacherModel
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    SubjectName = t.SubjectName,
                    Grade = 0
                }).ToList()
            };

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "1,3")]
        public ActionResult Create(StudentMakeModel student)
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

                    Teachers = student.TeachersList.Where(t => t.IsSelected).Select(t => new TeacherBLL
                    {
                        Id = t.Id,
                        UserId = t.UserId,
                        SubjectName = t.SubjectName,
                        Grade = t.Grade
                    })
                };

                _studentService.Create(studentBll);

                return Ok("Студент успешно зарегестирован");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult Show(int id)
        {
            var studentBll = _studentService.Get(id);

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
                }),

                AveradeScore = studentBll.AverageScore
            };

            return View(student);
        }

        [HttpGet]
        [Authorize(Roles = "1,3")]
        public ActionResult Edit(int id)
        {
            var studentBll = _studentService.Get(id);

            var student = new EditModel
            {
                Id = studentBll.Id,
                UserId = studentBll.UserId,
                FirstName = studentBll.User.FirstName,
                LastName = studentBll.User.LastName,
                Login = studentBll.User.Login,
                Password = studentBll.User.Password,

                TeachersList = _teacherService.GetAll().Select(t => new TeacherModel
                {
                    UserId = t.UserId,
                    Id = t.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    SubjectName = t.SubjectName,
                    Grade = t.Grade
                }).ToList()

            };

            foreach (var teacher in student.TeachersList)
            {
                if (studentBll.Teachers.Any(t => t.Id == teacher.Id))
                {
                    teacher.IsSelected = true;
                    teacher.Grade = studentBll.Teachers.First(t => t.Id == teacher.Id).Grade;
                }                    
            }

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "1,3")]
        public ActionResult Edit(EditModel student)
        {
            try
            {
                var userBll = new UserBLL
                {
                    Id = student.UserId,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Login = student.Login,
                    Password = student.Password
                };

                var studentBll = new StudentBLL
                {
                    Id = student.Id,
                    User = userBll,

                    Teachers = student.TeachersList.Select(t => new TeacherBLL
                    {
                        Id = t.Id,
                        UserId = t.UserId,
                        IsSelected = t.IsSelected,
                        Grade = t.Grade
                    })
                };

                _studentService.Update(studentBll);

                return Ok("Студент успешно изменен");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}