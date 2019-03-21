using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.Models.Student;
using Students_Site.Models.Subject;
using Students_Site.Models.Teacher;

namespace Students_Site.Controllers
{
    public class TeachersController : Controller
    {
        readonly ITeacherService _teacherService;
        readonly IUserService _userService;
        readonly IStudentService _studentService;
        readonly ISubjectService _subjectService;

        public TeachersController(ITeacherService teacherService, IUserService userService, ISubjectService subjectService, IStudentService studentService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _subjectService = subjectService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var teachersModels = _teacherService.GetTeachers().Select(s => new TeacherModel
            {
                Id = s.Id,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Login = s.User.Login,
                Password = s.User.Password,
                RoleId = s.User.RoleId,
                SubjectName = s.SubjectName,

                Students = s.Students.Select(t => new StudentModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            });

            var teachers = new TeacherIndexModel
            {
                TeacherModels = teachersModels
            };

            return View(teachers);
        }

        [HttpGet]
        public ActionResult MakeTeacher()
        {
            var teacher = new TeacherMakeModel
            {
                Students = _studentService.GetStudents().Select(s => new StudentModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    IsSelected = false
                }).ToList(),
                Subjects = _subjectService.GetSubjects().Select(sub => new SubjectModel
                {
                    Id = sub.Id,
                    SubjectName = sub.Name
                }).ToList(),

                SubjectId = 1
            };

            return View(teacher);
        }

        [HttpPost]
        public ActionResult MakeTeacher(TeacherMakeModel teacher)
        {
            try
            {
                var userBll = new UserBLL
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Login = teacher.Login,
                    Password = teacher.Password
                };

                var teacherBll = new TeacherBLL
                {
                    User = userBll,
                    SubjectId = teacher.SubjectId,

                    Students = teacher.Students.Where(s => s.IsSelected).Select(s => new StudentBLL
                    {
                        Id = s.Id,
                        UserId = s.UserId
                    })
                };

                _teacherService.MakeTeacher(teacherBll);

                return Content("Преподаватель успешно зарегестирован");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return Content(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult EditTeacher(int id)
        {
            var teacherBll = _teacherService.GetTeacher(id);

            var teacher = new TeacherEditModel
            {
                Id = teacherBll.Id,
                UserId = teacherBll.UserId,
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Password = teacherBll.User.Password,

                Students = _studentService.GetStudents().Select(s => new StudentModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    IsSelected = false
                }).ToList(),

                Subjects = _subjectService.GetSubjects().Select(sub => new SubjectModel
                {
                    Id = sub.Id,
                    SubjectName = sub.Name,
                }).ToList(),

            };

            foreach (var student in teacher.Students)
            {
                if (teacherBll.Students.Any(st => st.Id == student.Id))
                    student.IsSelected = true;
            }            

            return View(teacher);
        }

        [HttpPost]
        public ActionResult EditTeacher(TeacherEditModel teacher)
        {
            try
            {
                var userBll = new UserBLL
                {
                    Id = teacher.UserId,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Login = teacher.Login,
                    Password = teacher.Password
                };

                var teacherBll = new TeacherBLL
                {
                    Id = teacher.Id,
                    User = userBll,
                    SubjectId = teacher.SubjectId,

                    Students = teacher.Students.Select(s => new StudentBLL
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        IsSelected = s.IsSelected
                    })
                };

                _teacherService.UpdateTeacher(teacherBll);

                return Content("Преподаватель успешно изменен");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return Content(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _teacherService.Dispose();
            _userService.Dispose();
            base.Dispose(disposing);
        }

        public IActionResult ShowTeacher(int id)
        {
            var teacherBll = _teacherService.GetTeacher(id);

            var teacher = new TeacherModel
            {
                Id = teacherBll.Id,
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Password = teacherBll.User.Password,
                RoleId = teacherBll.User.RoleId,

                Students = teacherBll.Students.Select(t => new StudentModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            };

            return View(teacher);
        }
    }
}