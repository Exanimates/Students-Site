using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models;

namespace Students_Site.Controllers
{
    public class TeachersController : Controller
    {
        ITeacherService _teacherService;
        IUserService _userService;
        ISubjectService _subjectService;

        public TeachersController(ITeacherService teacherService, IUserService userService, ISubjectService subjectService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _subjectService = subjectService;
        }

        public IActionResult Index()
        {
            var teachers = _teacherService.GetTeachers().Select(s => new TeacherModel
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
            }).ToArray();

            return View(teachers);
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